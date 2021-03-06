using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;

namespace IconCMO
{
	
	public class DirectoryIndex
	{
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<DirectoryIndexEntry> entries;
			
		public DirectoryIndex(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			auth = _auth;
			filter = _filter;
			sort = _sort;
			entries = null;
		}

		public DirectoryIndex(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public void GetEntries()
		{
			xmlDoc = new XmlDocument();
			entries = new Collection<DirectoryIndexEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>directoryindex</Section>",
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
			LoadIndexEntries();
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
		
		public DirectoryIndexEntry()
		{
			Id = string.Empty;
			Status = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			Members = new Collection<DirectoryIndexMember>();
		}
		
	}
	
	public class DirectoryIndexMember
	{
		public string Id {get; set;}
		public string Status { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public DirectoryIndexMember()
		{
			Id = string.Empty;
			Status = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
		}
		
	}
	
	public class Directory
	{
				
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<DirectoryEntry> entries;
		private bool getNotes;
		private Collection<string> imageList;
		
		public Directory(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			
			auth = _auth;
			filter = _filter;
			sort = _sort;
			entries = null;
			imageList = new Collection<string>();
		}

		public Directory(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public Collection<DirectoryEntry> Entries
		{
			get { return entries; }
		}
		
		public bool Notes
		{
			get { return getNotes; }
			set { getNotes = value; }
		}
		
		public void GetEntries(string localImageCache)
		{
			xmlDoc = new XmlDocument();
			entries = new Collection<DirectoryEntry>();
			string section;
			
			// Include notes with the directory info?
			if (getNotes)
				section = "directorynotes";
			else
				section = "directory";
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>" + section + "</Section>",
														filter,
														auth,
														sort);
			// Send the request
			HttpWebResponse response = HttpRequestHandler.SendRequest(httpRequest);
			// Load the response into an XML document
			xmlDoc.Load(new StreamReader(response.GetResponseStream()));
			// Extract the security token from the response
			HttpRequestHandler.GetSession(xmlDoc, ref auth);
			// Load the directory entries
			LoadEntries(localImageCache);
			// Are we downloading images?
			if (localImageCache != null && localImageCache != string.Empty)
			{
				// Create the downloader class
				ImageDownloader imageDownloader = new ImageDownloader(imageList, localImageCache);
				Thread imageThread = new Thread(new ThreadStart(imageDownloader.DownloadImages));
				imageThread.Start();
			}
			
		}
		
		private void LoadEntries(string localImageCache)
		{
			string dirXPath; 
			
			// Did we get notes?
			if (getNotes)
				dirXPath = "/iconresponse/directorynotes";
			else
				dirXPath = "/iconresponse/directory";
				
			// Get the directory nodes
			XmlNodeList nodes = xmlDoc.SelectNodes(dirXPath);
			foreach (XmlNode node in nodes)
			{
				// Create a directory entry and add to the collection
				DirectoryEntry entry = new DirectoryEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("status");
				entry.Status = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("title");
				entry.Title = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("first_name");
				entry.FirstName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("last_name");
				entry.LastName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("mail_to");
				entry.MailTo = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("address_1");
				entry.Address1 = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("address_2");
				entry.Address2 = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("city");
				entry.City = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("state");
				entry.City = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("zip");
				entry.Zip = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("email");
				entry.Email = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("email_unlisted");
				Boolean wrkBool;
				Boolean.TryParse(wrkNode.InnerText, out wrkBool);
				entry.EmailUnlisted = wrkBool;
				wrkNode = node.SelectSingleNode("permission");
				Boolean.TryParse(wrkNode.InnerText, out wrkBool);
				entry.Permission = wrkBool;
				wrkNode = node.SelectSingleNode("phone");
				entry.Phone = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("phone_unlisted");
				Boolean.TryParse(wrkNode.InnerText, out wrkBool);
				entry.PhoneUnlisted = wrkBool;
				// Get the phones for the directory entry
				XmlNodeList phoneList = node.SelectNodes("phones");
				entry.Phones = LoadPhones(phoneList);
				// Get the emails for the directory entry
				XmlNodeList emailList = node.SelectNodes("emails");
				entry.Emails = LoadEmails(emailList);
				// Get the members for the directory index entry
				XmlNodeList memberNodes = node.SelectNodes("members");
				// Get the picture and thumbnail URLs
				wrkNode = node.SelectSingleNode("picture");
				entry.Picture = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("thumbnail");
				entry.Thumbnail = wrkNode.InnerText;
				// Are we downloading images?
				if (localImageCache != string.Empty) 
				{
					if (entry.Picture != null && entry.Picture != string.Empty)
						imageList.Add(entry.Picture);
					if (entry.Thumbnail != null && entry.Thumbnail != string.Empty)
						imageList.Add(entry.Thumbnail);
				}
				// Did we include notes?
				if (getNotes)
				{
					wrkNode = node.SelectSingleNode("notes");
					entry.Notes = wrkNode.InnerText;
					wrkNode = node.SelectSingleNode("notes_changed");
					DateTime wrkDate;
					DateTime.TryParse(wrkNode.InnerText, out wrkDate);
					entry.NotesChanged = wrkDate;
				}
				foreach (XmlNode memberNode in memberNodes)
				{
					DirectoryMember member = new DirectoryMember();
					wrkNode = memberNode.SelectSingleNode("id");
					member.Id = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("status");
					member.Status = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("title");
					member.Title = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("first_name");
					member.FirstName = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("last_name");
					member.LastName = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("work_phone");
					member.WorkPhone = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("work_phone_extension");
					member.WorkPhoneExtension = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("work_phone_unlisted");
					Boolean.TryParse(wrkNode.InnerText, out wrkBool);
					member.WorkPhoneUnlisted = wrkBool;
					wrkNode = memberNode.SelectSingleNode("birth_date");
					DateTime wrkDate;
					DateTime.TryParse(wrkNode.InnerText, out wrkDate);
					member.BirthDate = wrkDate;
					wrkNode = memberNode.SelectSingleNode("primary");
					Boolean.TryParse(wrkNode.InnerText, out wrkBool);
					member.Primary = wrkBool;
					// Get the phones for the directory entry
					phoneList = memberNode.SelectNodes("phones");
					member.Phones = LoadPhones(phoneList);
					// Get the emails for the directory entry
					emailList = memberNode.SelectNodes("emails");
					member.Emails = LoadEmails(emailList);
					// Get the picture and thumbnail URLs
					wrkNode = memberNode.SelectSingleNode("picture");
					member.Picture = wrkNode.InnerText;
					wrkNode = memberNode.SelectSingleNode("thumbnail");
					member.Thumbnail = wrkNode.InnerText;
					// Did we include notes?
					if (getNotes)
					{
						wrkNode = memberNode.SelectSingleNode("notes");
						member.Notes = wrkNode.InnerText;
						wrkNode = memberNode.SelectSingleNode("notes_changed");
						DateTime.TryParse(wrkNode.InnerText, out wrkDate);
						member.NotesChanged = wrkDate;
					}
					entry.Members.Add(member);
				}
				entries.Add(entry);
			}
			
		}
		
		private Collection<DirectoryPhone> LoadPhones(XmlNodeList phones)
		{
			Collection<DirectoryPhone> rtnPhones = new Collection<DirectoryPhone>();
			foreach (XmlNode phoneNode in phones)
			{
				if (phoneNode.ChildNodes.Count != 0)
				{
					DirectoryPhone phone = new DirectoryPhone();
					XmlNode wrkNode = phoneNode.SelectSingleNode("id");
					phone.Id = wrkNode.InnerText;
					wrkNode = phoneNode.SelectSingleNode("phone");
					phone.Phone = wrkNode.InnerText;
					wrkNode = phoneNode.SelectSingleNode("extension");
					phone.Extension = wrkNode.InnerText;
					wrkNode = phoneNode.SelectSingleNode("provider");
					phone.Provider = wrkNode.InnerText;
					wrkNode = phoneNode.SelectSingleNode("phone_unlisted");
					Boolean wrkBool;
					Boolean.TryParse(wrkNode.InnerText, out wrkBool);
					phone.PhoneUnlisted = wrkBool;
					rtnPhones.Add(phone);					
				}
			}
			return rtnPhones;
		}
			
		private Collection<DirectoryEmail> LoadEmails(XmlNodeList emails)
		{
			Collection<DirectoryEmail> rtnEmails = new Collection<DirectoryEmail>();
			foreach (XmlNode emailNode in emails)
			{
				if (emailNode.ChildNodes.Count != 0)
				{
					DirectoryEmail email = new DirectoryEmail();
					XmlNode wrkNode = emailNode.SelectSingleNode("id");
					email.Id = wrkNode.InnerText;
					wrkNode = emailNode.SelectSingleNode("email");
					email.Email = wrkNode.InnerText;
					wrkNode = emailNode.SelectSingleNode("email_unlisted");
					Boolean wrkBool;
					Boolean.TryParse(wrkNode.InnerText, out wrkBool);
					email.EmailUnlisted = wrkBool;
					rtnEmails.Add(email);					
				}
			}
			return rtnEmails;
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
	}
	
	public class DirectoryEntry
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MailTo { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Email { get; set; }
		public bool EmailUnlisted { get; set; }
		public bool Permission { get; set; }
		public string Phone { get; set; }
		public bool PhoneUnlisted { get; set; }
		public Collection<DirectoryMember> Members { get; set; }
		public Collection<DirectoryPhone> Phones { get; set; }
		public Collection<DirectoryEmail> Emails { get; set; }
		public string Picture { get; set; }
		public string Thumbnail { get; set; }
		public string Notes { get; set; }
		public DateTime NotesChanged { get; set; }
		public DirectoryEntry()
		{
			Id = string.Empty;
			Status = string.Empty;
			Title = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			MailTo = string.Empty;
			Address1 = string.Empty;
			Address2 = string.Empty;
			City = string.Empty;
			State = string.Empty;
			Zip = string.Empty;
			Email = string.Empty;
			EmailUnlisted = false;
			Permission = false;
			Phone = string.Empty;
			PhoneUnlisted = false;
			Members = new Collection<DirectoryMember>();
			Phones = new Collection<DirectoryPhone>();
			Emails = new Collection<DirectoryEmail>();
			Picture = string.Empty;
			Thumbnail = string.Empty;
			Notes = string.Empty;
			NotesChanged = DateTime.MinValue;
		}
		
	}
	
	public class DirectoryPhone 
	{
		public string Id { get; set; }
		public string Phone { get; set; }
		public string Extension { get; set; }
		public string Provider { get; set; }
		public bool PhoneUnlisted { get; set; }
		
		public DirectoryPhone()
		{
			Id = string.Empty;
			Phone = string.Empty;
			Extension = string.Empty;
			Provider = string.Empty;
			PhoneUnlisted = false;
		}
	
	}
	
	public class DirectoryEmail
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public bool EmailUnlisted { get; set; }
		public DirectoryEmail()
		{
			Id = string.Empty;
			Email = string.Empty;
			EmailUnlisted = false;
		}
	}

	public class DirectoryMember
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string WorkPhone { get; set; }
		public string WorkPhoneExtension { get; set; }
		public bool WorkPhoneUnlisted { get; set; }
		public DateTime BirthDate { get; set; }
		public bool Primary { get; set; }
		public string Picture { get; set; }
		public string Thumbnail { get; set; }
		public Collection<DirectoryPhone> Phones { get; set; }
		public Collection<DirectoryEmail> Emails { get; set; }
		public string Notes { get; set; }
		public DateTime NotesChanged { get; set; }
		public DirectoryMember()
		{
			Id = string.Empty;
			Status = string.Empty;	
			Title = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			WorkPhone = string.Empty;
			WorkPhoneExtension = string.Empty;
			WorkPhoneUnlisted = false;
			BirthDate = DateTime.MinValue;
			Primary = false;
			Picture = string.Empty;
			Thumbnail = string.Empty;
			Phones = new Collection<DirectoryPhone>();
			Emails = new Collection<DirectoryEmail>();
			Notes = string.Empty;
			NotesChanged = DateTime.MinValue;
		}
	}
			
}

