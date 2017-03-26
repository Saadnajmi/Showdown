﻿using System;
using System.Collections.Generic;
using Client.Common;
using Common.Common.Models;
using Foundation;
using UIKit;

namespace Client.iOS
{
	public class AnnoucementsViewController : UIViewController, IAnnouncementsView
	{
	    static NSString AnnouncmentCellID = new NSString("AnnouncementCellID");

	    public UITableView AnnouncementsList { get; set; }

		public AnnouncementsPresenter Presenter { get; set; }

	    public AnnoucementsViewController()
		{
		    var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
		    Presenter = new AnnouncementsPresenter(appDelegate.BackendClient);
			Presenter.TakeView(this);
		}

	    public List<Announcement> Announcements
	    {
	        set
	        {
                AnnouncementsTableSource ats = AnnouncementsList.Source as AnnouncementsTableSource;
	            ats.Announcements = value;
	            AnnouncementsList.ReloadData();
	        }
	    }

	    public async override void ViewDidLoad()
	    {
	        base.ViewDidLoad();
	        View.BackgroundColor = UIColor.White;

	        AnnouncementsList = new UITableView(View.Bounds)
	        {
                BackgroundColor = UIColor.Clear,
	            Source = new AnnouncementsTableSource(),
	            RowHeight = 150,
	            SeparatorStyle = UITableViewCellSeparatorStyle.None
	        };
			AnnouncementsList.RegisterClassForCellReuse(typeof(AnnouncementCell), AnnouncmentCellID);

	        View.AddSubview(AnnouncementsList);
			await Presenter.OnBegin();
	    }

	    public override async void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);
			Presenter.TakeView(this);
	
	    }

	    public override void ViewWillDisappear(bool animated)
	    {
	        base.ViewWillDisappear(animated);
	        Presenter.RemoveView();
	    }

	    class AnnouncementsTableSource : UITableViewSource
		{
			public List<Announcement> Announcements { get; set; }

		    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(AnnouncmentCellID) as AnnouncementCell;
				cell.BackgroundColor = UIColor.Clear;

				var announcement = Announcements[indexPath.Row];

				cell.UpdateCell(announcement);

				return cell;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
			    return Announcements?.Count ?? 0;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				tableView.DeselectRow(indexPath, false);
			}
		}
	}
}
