﻿using System;
using Common.Common.Models;

namespace Client.Common
{
	public interface IGameHavingPresenter
	{
		Game GetGame(int index);
		int GameCount();
		void GameTapped(int index);
	}
}
