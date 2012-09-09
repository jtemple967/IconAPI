using System;

namespace IconAPI
{
	public class IconFilter
	{
		
		private string[] _id = null; 
		private int? _startAt = null;
		private int? _limit = null;
		
		public IconFilter(string[] id, int startAt, int limit)
		{
			_id = id;
			_startAt = startAt;
			_limit = limit;
		}
		
		public IconFilter(string[] id)
		{
			_id = id;
		}
		
		public IconFilter(int startAt, int limit)
		{
			_startAt = startAt;
			_limit = limit;
		}
		
		public string[] Id
		{
			get { return _id; }
			set { _id = value; }
		}
		
		public int? StartAt
		{
			get { return _startAt; } 
			set { _startAt = value; }
		}
		
		public int? Limit
		{
			get { return _limit; }
			set { _limit = value; }
		}
		
		public string GetFilterXML()
		{
			string filter = string.Empty;

			filter = "<Filters>";
			if (_id != null)
			foreach (String id in _id)
				filter += "<id>" + id.ToString() + "</id>";
			if (_startAt != null)
				filter += "<startAt>" + _startAt.ToString() + "</startAt>";
			if (_limit != null)
				filter += "<limit>" + _limit.ToString() + "</limit>";
			filter += "</Filters>";
			
			return filter;	
			
		}
	}
}

