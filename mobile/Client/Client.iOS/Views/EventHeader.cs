﻿using System;
using Common.Common;
using Common.Common.Models;
using Foundation;
using Masonry;
using UIKit;

namespace Client.iOS
{
	public class EventHeader : UIView
	{
		static UIFont titleFont = UIFont.SystemFontOfSize(18, UIFontWeight.Bold);
		static UIFont timeFont = UIFont.SystemFontOfSize(16, UIFontWeight.Semibold);
		static UIFont locationFont = UIFont.SystemFontOfSize(16, UIFontWeight.Medium);
		static UIFont descriptionFont = UIFont.SystemFontOfSize(14, UIFontWeight.Regular);

		UILabel titleLabel = new UILabel() { Font = titleFont, TextAlignment = UITextAlignment.Center };
		UILabel timeLabel = new UILabel() { Font = timeFont, TextAlignment = UITextAlignment.Center };
		UIButton locationLabel = new UIButton()
		{
			BackgroundColor = UIColor.White
		};

		UILabel descriptionLabel = new UILabel()
		{
			Font = descriptionFont,
			Lines = 3,
			TextAlignment = UITextAlignment.Center
		};

		public Action LocationTappedAction { get; set; }

		public EventHeader()
		{
			BackgroundColor = UIColor.White;
			Layer.BorderColor = UIColor.LightGray.CGColor;
			Layer.BorderWidth = 0.5f;

			locationLabel.TitleLabel.Font = locationFont;
			locationLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
			locationLabel.SetTitleColor(UIColor.LightGray, UIControlState.Highlighted);
			locationLabel.TouchUpInside += (sender, e) => LocationTappedAction();

			AddSubviews(new UIView[] {
				titleLabel,
				timeLabel,
				locationLabel,
				descriptionLabel
			});

			var parentView = this;

			titleLabel.MakeConstraints(make =>
			{
				make.Left.EqualTo(parentView);
				make.Top.EqualTo(parentView);
				make.Width.EqualTo(parentView);
			});

			timeLabel.MakeConstraints(make =>
			{
				make.Left.EqualTo(titleLabel);
				make.Top.EqualTo(titleLabel.Bottom());
				make.Width.EqualTo(parentView);
			});

			locationLabel.MakeConstraints(make =>
			{
				make.CenterX.EqualTo(titleLabel);
				make.Top.EqualTo(timeLabel.Bottom());
				make.Width.EqualTo(parentView);
				make.Height.EqualTo((NSNumber)40);
			});
			locationLabel.ContentEdgeInsets = new UIEdgeInsets(0.01f, 0.01f, 0.01f, 0.01f);

			descriptionLabel.MakeConstraints(make =>
			{
				make.CenterX.EqualTo(parentView);
				make.Width.EqualTo(parentView).MultipliedBy(0.95f);;
				make.Top.EqualTo(locationLabel.Bottom());
			});
		}

		public Event Event
		{
			set
			{
				titleLabel.Text = value.Title;
				timeLabel.Text = 
					$"{Utilities.FormatEventTime(value.StartTime)} - {Utilities.FormatEventTime(value.EndTime)}";
				locationLabel.SetTitle(value.Location.Name, UIControlState.Normal);
				descriptionLabel.Text = value.Description;
			}
		}
	}
}
