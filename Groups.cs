using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Xml;

namespace IconCMO
{
	public class GroupMember
	{
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<GroupCategoryEntry> entries;
			
		public GroupMember(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			auth = _auth;
			filter = _filter;
			sort = _sort;
			entries = null;
		}

		public GroupMember(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public void GetEntries()
		{
			xmlDoc = new XmlDocument();
			entries = new Collection<GroupCategoryEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>groups</Module>" +
														"<Section>member</Section>",
														filter,
														auth,
														sort);
			// Send the request
			HttpWebResponse response = HttpRequestHandler.SendRequest(httpRequest);
			// Load the response into an XML document
			xmlDoc.Load(new StreamReader(response.GetResponseStream()));
			// Extract the security token from the response
			HttpRequestHandler.GetSession(xmlDoc, ref auth);
			// Load the directory index entries
			LoadMemberEntries();
		}	
			
		public Collection<GroupCategoryEntry> Entries
		{
			get { return entries; }
		}
		
		private void LoadMemberEntries()
		{
			// Get the memberindex nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/member");
			foreach (XmlNode node in nodes)
			{
				// Create a group category entry and add to the collection
				GroupCategoryEntry entry = new GroupCategoryEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("category");
				entry.Category = wrkNode.InnerText;
				
				// Now load the groups in this category
				XmlNodeList groupList = node.SelectNodes("groups");
				foreach (XmlNode groupNode in groupList)
				{
					GroupEntry groupEntry = new GroupEntry();
					wrkNode = groupNode.SelectSingleNode("id");
					groupEntry.Id = wrkNode.InnerText;
					wrkNode = groupNode.SelectSingleNode("name");
					groupEntry.Name = wrkNode.InnerText;
					wrkNode = groupNode.SelectSingleNode("description");
					groupEntry.Description = wrkNode.InnerText;
					// Get the members in the group
					XmlNodeList memberList = groupNode.SelectNodes("members");
					if (memberList.Count > 0)
					{
						foreach (XmlNode memberNode in memberList)
						{
							if (memberNode.ChildNodes.Count > 0 )
							{
								GroupMemberEntry memberEntry = new GroupMemberEntry();
								wrkNode = memberNode.SelectSingleNode("id");
								memberEntry.Id = wrkNode.InnerText;
								wrkNode = memberNode.SelectSingleNode("first_name");
								memberEntry.FirstName = wrkNode.InnerText;
								wrkNode = memberNode.SelectSingleNode("last_name");
								memberEntry.LastName = wrkNode.InnerText;
								groupEntry.Members.Add(memberEntry);
							}
						}
					}
					entry.Groups.Add(groupEntry);
				}
				entries.Add(entry);
			}
			
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
		
	}
	
	public class GroupCategoryEntry
	{
		public string Id { get; set; }
		public string Category { get; set; }
		public Collection<GroupEntry> Groups { get; set; } 
		
		public GroupCategoryEntry()
		{
			Id = string.Empty;
			Category = string.Empty;
			Groups = new Collection<GroupEntry>();
		}
		
	}

	public class GroupEntry
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Collection<GroupMemberEntry> Members { get; set; }
		
		public GroupEntry()
		{
			Id = string.Empty;
			Name = string.Empty;
			Description = string.Empty;
			Members = new Collection<GroupMemberEntry>();
		}
	}
	
	public class GroupMemberEntry
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public GroupMemberEntry()
		{
			Id = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
		}
	}
		
	
}

