﻿using System;
using System.Collections.Generic;
using Client.Common;
using Common.Common.Models;
using Foundation;
using UIKit;
using Masonry;

namespace Client.iOS
{
	public class GameViewController : UIViewController, IGameView
	{
		static NSString ScoreCellID = new NSString("ScoreCellId");

		GamePresenter Presenter { get; set; }
		public Game Game { get; set; }
		UITableView ScoresList { get; set; }

		GameHeader Header { get; set; }

		NSTimer updateTimer;

		public GameViewController(Game g)
		{
			var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
			Presenter = new GamePresenter(appDelegate.BackendClient, appDelegate.SubscriptionManager) { Game = g };

			Game = g;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
			var tabBarController = appDelegate.Window.RootViewController as UITabBarController;
			var navController = tabBarController.SelectedViewController as UINavigationController;

			navController.NavigationBar.Translucent = false;

			View.BackgroundColor = Resources.Colors.backgroundColor;
            Header = new GameHeader()
            {
                AwayTeamAction = () => { navController.PushViewController(new SchoolViewController(Game.AwayTeam), true); },
                HomeTeamAction = () => { navController.PushViewController(new SchoolViewController(Game.HomeTeam), true); },
                EventAction = () => { navController.PushViewController(new EventViewController(Game.Event), true); },
                NotificationTappedAction = async () => await Presenter.OnStar()
			};

			ScoresList = new UITableView()
			{
				BackgroundColor = UIColor.Clear,
				Source = new ScoreTableSource() { Game = Game},
				RowHeight = 50,
				SeparatorStyle = UITableViewCellSeparatorStyle.None
			};
			ScoresList.RegisterClassForCellReuse(typeof(ScoreCell), ScoreCellID);

			View.Add(Header);
			View.Add(ScoresList);


			Header.MakeConstraints(make =>
			{
				make.Height.EqualTo((NSNumber)160);
				make.Top.EqualTo(View);
				make.Left.EqualTo(View);
				make.Width.EqualTo(View);
			});

			ScoresList.MakeConstraints(make =>
			{
				make.Top.EqualTo(Header.Bottom());
				make.Bottom.EqualTo(View).Offset(-49);
				make.Width.EqualTo(View);
			});

			Header.LayoutView();

		}

		public async override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			Presenter.TakeView(this);
			updateTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(5), async (obj) => await Presenter.OnTick());

			var updateTask = Presenter.OnBegin();

		    Header.Game = Game;
			Header.IsSubscribed = Presenter.IsSubscribed();

			await updateTask;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			Presenter.RemoveView();
			updateTimer.Invalidate();
		}

		public List<Score> ScoreHistory
		{
			set
			{
				ScoreTableSource sts = ScoresList.Source as ScoreTableSource;
				sts.ScoreHistory = value;
				ScoresList.ReloadData();

				var game = Game;
				if (value.Count > 0)
				{
					game.Score = value[0];
					Header.Game = game;
					Game = game;
				}
			}
		}

		public void ShowMessage(string message)
		{
			var alertView = new UIAlertView("", message, null, "OK", new string[] { });
			alertView.Show();
		}

		public void Refresh()
		{
			Header.IsSubscribed = Presenter.IsSubscribed();
			ScoresList.ReloadData();
		}

		class ScoreTableSource : UITableViewSource
		{
		    public Game Game;
			public List<Score> ScoreHistory { get; set; }

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(ScoreCellID) as ScoreCell;
				cell.BackgroundColor = UIColor.Clear;

				var scoreRecord = ScoreHistory[indexPath.Row];

				cell.UpdateCell(Game, scoreRecord);

				return cell;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return ScoreHistory.Count;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				tableView.DeselectRow(indexPath, false);
			}
		}
	}
}
