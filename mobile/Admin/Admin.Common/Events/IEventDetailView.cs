﻿using System;
using System.Collections.Generic;
using Admin.Common.API.Entities;

namespace Admin.Common
{
	public interface IEventDetailView
	{
		Event Event { get; }

		List<string> LocationOptions { set; }
		int SelectedLocationIndex { get; set; }

		bool LocationLoading { set; }
	}
}
