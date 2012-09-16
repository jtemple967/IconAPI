using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace IconCMO
{
	public class IconAPI
	{
		private IconAuth auth = null;
		private Permissions permissions = null;
		private DirectoryIndex dirIndex = null;
		
		public IconAPI (IconAuth _auth)
		{
			auth = _auth;
			// Load the permissions once for this users
			permissions = new Permissions(auth);
			// Get the returned authorization structure once. The session token 
			// should now be set
			auth = permissions.Auth;
			
		}
		
		public Collection<DirectoryIndexEntry> GetDirectoryIndexEntries(IconFilter _filter)
		{
			dirIndex = new DirectoryIndex(auth, _filter);
			return dirIndex.Entries;
		}
			
		public Collection<Permission> Permissions
		{
			get { return permissions.Entries; }
		}
	}
}

