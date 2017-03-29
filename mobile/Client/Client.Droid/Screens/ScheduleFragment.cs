using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Client.Droid.Adapters;
using Common.Common;
using Client.Common;
using Common.Common.Models;
using System;
using System.Collections.Generic;

namespace Client.Droid.Screens
{
    public class ScheduleFragment : Fragment, IScheduleView
    {
        SchedulePresenter Presenter { get; set; }


        List<Event> IScheduleView.Events {
            set
            {
                ScheduleAdapter adapter = this.Adapter;
				if (adapter == null) return;
                adapter.Events = value;
                adapter.NotifyDataSetChanged();
            }
        }
        RecyclerView ScheduleView { get; set; }
        ScheduleAdapter Adapter { get; set; }

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Presenter = new SchedulePresenter(new ShowdownRESTClient());
            Presenter.TakeView(this);

            Adapter = new ScheduleAdapter()
            {
                Events = new List<Event>()
            };
            Adapter.ItemClick += (object sender, ScheduleAdapterClickEventArgs args) => Presenter.OnClickRow(args.Event);
            await Presenter.OnBegin();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_schedule, container, false);

            // Set up Recycler View for the Schedule
            ScheduleView = view.FindViewById<RecyclerView>(Resource.Id.scheduleRecyclerView);
            ScheduleView.SetLayoutManager(new LinearLayoutManager(this.Activity));
            ScheduleView.SetAdapter(Adapter);

            return view;
        }

        void IScheduleView.ShowMessage(string message)
        {
            Toast.MakeText(this.Activity, message, ToastLength.Short).Show();
        }

        void IScheduleView.OpenEvent(Event row)
        {
            var Intent = new Intent(this.Activity, typeof(DetailedEventActivity));
            Intent.PutExtra("event", row.ToJSON());
            StartActivity(Intent);
        }
    }
}

