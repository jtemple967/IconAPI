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
		private Directory directory = null;
		private HouseholdIndex houseIndex = null;
		
		public IconAPI (IconAuth _auth)
		{
			auth = _auth;
			// Load the permissions once for this users
			permissions = new Permissions(auth);
			// Get the returned authorization structure once. The session token 
			// should now be set
			auth = permissions.Auth;
			
		}
		
		public Collection<DirectoryIndexEntry> GetDirectoryIndexEntries(IconFilter _filter, IconSort _sort)
		{
			dirIndex = new DirectoryIndex(auth, _filter, _sort);
			dirIndex.GetEntries();
			return dirIndex.Entries;
		}
			
		public Collection<DirectoryEntry> GetDirectoryEntries(IconFilter _filter, IconSort _sort)
		{
			directory = new Directory(auth, _filter, _sort);
			directory.GetEntries();
			return directory.Entries;
		}
		
		public Collection<DirectoryEntry> GetDirectoryEntriesNotes(IconFilter _filter, IconSort _sort)
		{
			directory = new Directory(auth, _filter, _sort);
			directory.Notes = true;
			directory.GetEntries();
			return directory.Entries;
		}
		
		public Collection<HouseholdIndexEntry> GetHouseholdIndexEntries(IconFilter _filter, IconSort _sort)
		{
			HouseholdIndex houseIndex = new HouseholdIndex(auth, _filter, _sort);
			houseIndex.GetEntries();
			return houseIndex.Entries;
		}
			
		public Collection<Permission> Permissions
		{
			get { return permissions.Entries; }
		}
	}
}

