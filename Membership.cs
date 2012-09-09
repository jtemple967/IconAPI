using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Xml;

namespace IconAPI
{
	
	public class Directoryindex
	{
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private Collection<DirectoryIndexEntry> entries;
			
		public Directoryindex(IconAuth _auth, IconFilter _filter)
		{
			auth = _auth;
			filter = _filter;
			xmlDoc = new XmlDocument();
			entries = new Collection<DirectoryIndexEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>directoryindex</Section>",
														filter,
														auth);
			// Send the request
			HttpWebResponse response = HttpRequestHandler.SendRequest(httpRequest);
			// Load the response into an XML document
			xmlDoc.Load(new StreamReader(response.GetResponseStream()));
			// Extract the security token from the response
			HttpRequestHandler.GetSession(xmlDoc, ref auth);
			// Load the directory index entries
			LoadIndexEntries();
			
		}

		public Directoryindex(IconAuth _auth) : this(_auth, null)
		{
			
		}
		
		public Collection<DirectoryIndexEntry> Entries
		{
			get { return entries; }
		}
		
		private void LoadIndexEntries()
		{
			// Get the directoryindex nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/directoryindex");
			foreach (XmlNode node in nodes)
			{
				// Create a directory index entry and add to the collection
				DirectoryIndexEntry entry = new DirectoryIndexEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("status");
				entry.Status = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("first_name");
				entry.FirstName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("last_name");
				entry.LastName = wrkNode.InnerText;
				// Get the members for the directory index entry
				XmlNodeList memberNodes = node.SelectNodes("members");
				foreach (XmlNode memberNode in memberNodes)
				{
					DirectoryIndexMember member = new DirectoryIndexMember();
					wrkNode = node.SelectSingleNode("id");
					member.Id = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("status");
					member.Status = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("first_name");
					member.FirstName = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("last_name");
					member.LastName = wrkNode.InnerText;
					entry.Members.Add(member);
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
	
	public class DirectoryIndexEntry
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Collection<DirectoryIndexMember> Members { get; set; }
		
		public DirectoryIndexEntry(string _id = "", string _status = "", 
									string _firstName = "", string _lastName = "")
		{
			Id = _id;
			Status = _status;
			FirstName = _firstName;
			LastName = _lastName;
			Members = new Collection<DirectoryIndexMember>();
		}
		
	}
	
	public class DirectoryIndexMember
	{
		public string Id {get; set;}
		public string Status { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public DirectoryIndexMember(string _id = "", string _status = "", 
									string _firstName = "", string _lastName = "")
		{
			Id = _id;
			Status = _status;
			FirstName = _firstName;
			LastName = _lastName;
		}
		
	}
	
}

