using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Xml;

namespace IconCMO
{
	
	public class HouseholdIndex
	{
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<HouseholdIndexEntry> entries;
			
		public HouseholdIndex(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			auth = _auth;
			filter = _filter;
			sort = _sort;
			entries = null;
		}

		public HouseholdIndex(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public void GetEntries()
		{
			xmlDoc = new XmlDocument();
			entries = new Collection<HouseholdIndexEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>householdindex</Section>",
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
			
		public Collection<HouseholdIndexEntry> Entries
		{
			get { return entries; }
		}
		
		private void LoadIndexEntries()
		{
			// Get the householdindex nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/householdindex");
			foreach (XmlNode node in nodes)
			{
				// Create a directory index entry and add to the collection
				HouseholdIndexEntry entry = new HouseholdIndexEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("status");
				entry.Status = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("first_name");
				entry.FirstName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("last_name");
				entry.LastName = wrkNode.InnerText;
				entries.Add(entry);
			}
			
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
	}
	
	public class HouseholdIndexEntry
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		public HouseholdIndexEntry()
		{
			Id = string.Empty;
			Status = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
		}
		
	}

	public class Household
	{
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<HouseholdEntry> entries;
			
		public Household(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			auth = _auth;
			filter = _filter;
			sort = _sort;
			entries = null;
		}

		public Household(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public void GetEntries()
		{
			xmlDoc = new XmlDocument();
			entries = new Collection<HouseholdEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>households</Section>",
														filter,
														auth,
														sort);
			// Send the request
			HttpWebResponse response = HttpRequestHandler.SendRequest(httpRequest);
			// Load the response into an XML document
			xmlDoc.Load(new StreamReader(response.GetResponseStream()));
			// Extract the security token from the response
			HttpRequestHandler.GetSession(xmlDoc, ref auth);
			// Load the household entries
			LoadEntries();
		}	
			
		public Collection<HouseholdEntry> Entries
		{
			get { return entries; }
		}
		
		private void LoadEntries()
		{
			// Get the householdindex nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/households");
			foreach (XmlNode node in nodes)
			{
				// Create a household entry and add to the collection
				HouseholdEntry entry = new HouseholdEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				DateTime wrkDate;
				wrkNode = node.SelectSingleNode("date_changed");
				DateTime.TryParse(wrkNode.InnerText, out wrkDate);
				entry.DateChanged = wrkDate;
				wrkNode = node.SelectSingleNode("status");
				entry.Status = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("status_date");
				DateTime.TryParse(wrkNode.InnerText, out wrkDate);
				entry.StatusDate = wrkDate;
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
				entry.State = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("zip");
				entry.Zip = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("country");
				entry.Country = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("carrier_route");
				entry.CarrierRoute = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("zone");
				entry.Zone = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("email");
				entry.Email = wrkNode.InnerText;
				bool wrkBool;
				wrkNode = node.SelectSingleNode("email_unlisted");
				bool.TryParse(wrkNode.InnerText, out wrkBool);
				entry.EmailUnlisted = wrkBool;
				wrkNode = node.SelectSingleNode("permission");
				bool.TryParse(wrkNode.InnerText, out wrkBool);
				entry.Permission = wrkBool;
				wrkNode = node.SelectSingleNode("phone");
				entry.Phone = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("phone_unlisted");
				bool.TryParse(wrkNode.InnerText, out wrkBool);
				entry.PhoneUnlisted = wrkBool;
				// Get the phones for the household entry
				XmlNodeList phoneList = node.SelectNodes("phones");
				entry.Phones = LoadPhones(phoneList);
				// Get the emails for the household entry
				XmlNodeList emailList = node.SelectNodes("emails");
				entry.Emails = LoadEmails(emailList);
				// Get the members for the household entry
				XmlNodeList memberNodes = node.SelectNodes("members");
				foreach (XmlNode memberNode in memberNodes)
				{
					HouseholdMember member = new HouseholdMember();
					wrkNode = memberNode.SelectSingleNode("id");
					member.Id = wrkNode.InnerText;
					entry.Members.Add(member);
				}
				// Get the user defined fields
				wrkNode = node.SelectSingleNode("user_defined_1");
				entry.UserDefined1 = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("user_defined_2");
				entry.UserDefined2 = wrkNode.InnerText;
				// Get the picture and thumbnail URLs
				wrkNode = node.SelectSingleNode("picture");
				entry.Picture = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("thumbnail");
				entry.Thumbnail = wrkNode.InnerText;				
				entries.Add(entry);
			}
			
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
		
		private Collection<HouseholdPhone> LoadPhones(XmlNodeList phones)
		{
			Collection<HouseholdPhone> rtnPhones = new Collection<HouseholdPhone>();
			foreach (XmlNode phoneNode in phones)
			{
				if (phoneNode.ChildNodes.Count != 0)
				{
					HouseholdPhone phone = new HouseholdPhone();
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
					DateTime wrkDateTime;
					wrkNode = phoneNode.SelectSingleNode("date_changed");
					DateTime.TryParse(wrkNode.InnerText, out wrkDateTime);
					phone.DateChanged = wrkDateTime;
					rtnPhones.Add(phone);					
				}
			}
			return rtnPhones;
		}
			
		private Collection<HouseholdEmail> LoadEmails(XmlNodeList emails)
		{
			Collection<HouseholdEmail> rtnEmails = new Collection<HouseholdEmail>();
			foreach (XmlNode emailNode in emails)
			{
				if (emailNode.ChildNodes.Count != 0)
				{
					HouseholdEmail email = new HouseholdEmail();
					XmlNode wrkNode = emailNode.SelectSingleNode("id");
					email.Id = wrkNode.InnerText;
					wrkNode = emailNode.SelectSingleNode("email");
					email.Email = wrkNode.InnerText;
					wrkNode = emailNode.SelectSingleNode("email_unlisted");
					Boolean wrkBool;
					Boolean.TryParse(wrkNode.InnerText, out wrkBool);
					email.EmailUnlisted = wrkBool;
					DateTime wrkDateTime;
					wrkNode = emailNode.SelectSingleNode("date_changed");
					DateTime.TryParse(wrkNode.InnerText, out wrkDateTime);
					email.DateChanged = wrkDateTime;					
					rtnEmails.Add(email);					
				}
			}
			return rtnEmails;
		}
		
		
	}
	
	public class HouseholdEntry
	{
		public string Id { get; set; }
		public DateTime DateChanged { get; set; }
		public string Status { get; set; }
		public DateTime StatusDate { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MailTo { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public string CarrierRoute { get; set; }
		public string Zone { get; set; }
		public string Email { get; set; }
		public bool EmailUnlisted { get; set; }
		public bool Permission { get; set; }
		public string Phone { get; set; }
		public bool PhoneUnlisted { get; set; }
		public string UserDefined1 { get; set; }
		public string UserDefined2 { get; set; }
		public string Picture { get; set; }
		public string Thumbnail { get; set; }
		public Collection<HouseholdPhone> Phones { get; set; }
		public Collection<HouseholdEmail> Emails { get; set; }
		public Collection<HouseholdMember> Members { get; set; }
		
		public HouseholdEntry()
		{
			Id = string.Empty;
			DateChanged = DateTime.MinValue;
			Status = string.Empty;
			StatusDate = DateTime.MinValue;
			Title = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			MailTo = string.Empty;
			Address1 = string.Empty;
			Address2 = string.Empty;
			City = string.Empty;
			State = string.Empty;
			Zip = string.Empty;
			Country = string.Empty;
			CarrierRoute = string.Empty;
			Zone = string.Empty;
			Email = string.Empty;
			EmailUnlisted = false;
			Permission = false;
			Phone = string.Empty;
			PhoneUnlisted = false;
			UserDefined1 = string.Empty;
			UserDefined2 = string.Empty;
			Picture = string.Empty;
			Thumbnail = string.Empty;
			Phones = new Collection<HouseholdPhone>();
			Emails = new Collection<HouseholdEmail>();
			Members = new Collection<HouseholdMember>();
		}
		
	}

	public class HouseholdPhone
	{
		public string Id { get; set; }
		public string Phone { get; set; }
		public string Extension { get; set; }
		public string Provider { get; set; }
		public bool PhoneUnlisted { get; set; }
		public DateTime DateChanged { get; set; }
		
		public HouseholdPhone()
		{
			Id = string.Empty;
			Phone = string.Empty;
			Extension = string.Empty;
			Provider = string.Empty;
			PhoneUnlisted = false;
			DateChanged = DateTime.MinValue;
		}
	}
	
	public class HouseholdEmail
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public bool EmailUnlisted { get; set; }
		public DateTime DateChanged { get; set; }
		
		public HouseholdEmail()
		{
			Id = string.Empty;
			Email = string.Empty;
			EmailUnlisted = false;
			DateChanged = DateTime.MinValue;
		}
	}

	public class HouseholdMember
	{
		public string Id { get; set; }
	
		public HouseholdMember()
		{
			Id = string.Empty;
		}
	}
	
}