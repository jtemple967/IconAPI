using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Xml;

namespace IconCMO
{
	public class MemberIndex
	{
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<MemberIndexEntry> entries;
			
		public MemberIndex(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			auth = _auth;
			filter = _filter;
			sort = _sort;
			entries = null;
		}

		public MemberIndex(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public void GetEntries()
		{
			xmlDoc = new XmlDocument();
			entries = new Collection<MemberIndexEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>memberindex</Section>",
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
			
		public Collection<MemberIndexEntry> Entries
		{
			get { return entries; }
		}
		
		private void LoadIndexEntries()
		{
			// Get the memberindex nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/memberindex");
			foreach (XmlNode node in nodes)
			{
				// Create a member index entry and add to the collection
				MemberIndexEntry entry = new MemberIndexEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("household_id");
				entry.HouseholdId = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("status");
				entry.Status = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("first_name");
				entry.FirstName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("last_name");
				entry.LastName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("preferred_name");
				entry.PreferredName = wrkNode.InnerText;
				entries.Add(entry);
			}
			
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
	}
	
	public class MemberIndexEntry
	{
		public string Id { get; set; }
		public string HouseholdId { get; set; }
		public string Status { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PreferredName { get; set; }
		
		public MemberIndexEntry()
		{
			Id = string.Empty;
			HouseholdId = string.Empty;
			Status = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			PreferredName = string.Empty;
		}
		
	}

	public class Member
	{
				
		private IconAuth auth;
		private string httpRequest;
		private XmlDocument xmlDoc;
		private IconFilter filter;
		private IconSort sort;
		private Collection<MemberEntry> entries;
		
		public Member(IconAuth _auth, IconFilter _filter, IconSort _sort)
		{
			
			auth = _auth;
			filter = _filter;
			sort = _sort;
			entries = null;
			
		}

		public Member(IconAuth _auth) : this(_auth, null, null)
		{
			
		}
		
		public Collection<MemberEntry> Entries
		{
			get { return entries; }
		}
		
		public void GetEntries()
		{
			xmlDoc = new XmlDocument();
			entries = new Collection<MemberEntry>();
			
			// Form the http request
			httpRequest = HttpRequestHandler.BuildRequest("<Module>membership</Module>" +
														"<Section>members</Section>",
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
			LoadEntries();
			
		}
		
		private void LoadEntries()
		{
				
			// Get the member nodes
			XmlNodeList nodes = xmlDoc.SelectNodes("/iconresponse/members");
			foreach (XmlNode node in nodes)
			{
				// Create a member entry and add to the collection
				MemberEntry entry = new MemberEntry();
				XmlNode wrkNode = node.SelectSingleNode("id");
				entry.Id = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("household_id");
				entry.HouseholdId = wrkNode.InnerText;
				DateTime wrkDate;
				wrkNode = node.SelectSingleNode("status_date");
				DateTime.TryParse(wrkNode.InnerText, out wrkDate);
				entry.StatusDate = wrkDate;
				wrkNode = node.SelectSingleNode("title");
				entry.Title = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("first_name");
				entry.FirstName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("middle_name");
				entry.MiddleName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("last_name");
				entry.LastName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("preferred_name");
				entry.PreferredName = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("maiden_name");
				entry.MaidenName = wrkNode.InnerText;
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
				wrkNode = node.SelectSingleNode("phone");
				entry.Phone = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("occupation");
				entry.Occupation = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("workplace");
				entry.WorkPlace = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("work_phone");
				entry.WorkPhone = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("work_phone_extension");
				entry.WorkPhoneExtension = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("work_phone_unlisted");
				Boolean wrkBool;
				Boolean.TryParse(wrkNode.InnerText, out wrkBool);
				entry.WorkPhoneUnlisted = wrkBool;
				wrkNode = node.SelectSingleNode("primary");
				Boolean.TryParse(wrkNode.InnerText, out wrkBool);
				entry.Primary = wrkBool;
				wrkNode = node.SelectSingleNode("gender");
				entry.Gender = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("relationship");
				entry.Relationship = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("primary_language");
				entry.PrimaryLanguage = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("secondary_language");
				entry.SecondaryLanguage = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("ethnicity");
				entry.Ethnicity = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("denomination");
				entry.Denomination = wrkNode.InnerText;
				// Get the phones for the member entry
				XmlNodeList phoneList = node.SelectNodes("phones");
				entry.Phones = LoadPhones(phoneList);
				// Get the emails for the member entry
				XmlNodeList emailList = node.SelectNodes("emails");
				entry.Emails = LoadEmails(emailList);
				// Get the special dates for the member entry
				XmlNodeList dateList = node.SelectNodes("special_dates");
				entry.SpecialDates = LoadDates(dateList);
				// Get the picture and thumbnail URLs
				wrkNode = node.SelectSingleNode("picture");
				entry.Picture = wrkNode.InnerText;
				wrkNode = node.SelectSingleNode("thumbnail");
				entry.Thumbnail = wrkNode.InnerText;
				entries.Add(entry);
			}
			
		}
		
		private Collection<MemberPhone> LoadPhones(XmlNodeList phones)
		{
			Collection<MemberPhone> rtnPhones = new Collection<MemberPhone>();
			foreach (XmlNode phoneNode in phones)
			{
				if (phoneNode.ChildNodes.Count != 0)
				{
					MemberPhone phone = new MemberPhone();
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
			
		private Collection<MemberEmail> LoadEmails(XmlNodeList emails)
		{
			Collection<MemberEmail> rtnEmails = new Collection<MemberEmail>();
			foreach (XmlNode emailNode in emails)
			{
				if (emailNode.ChildNodes.Count != 0)
				{
					MemberEmail email = new MemberEmail();
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
		
		private Collection<MemberSpecialDate> LoadDates(XmlNodeList dates)
		{
			Collection<MemberSpecialDate> rtnDates = new Collection<MemberSpecialDate>();
			foreach (XmlNode dateNode in dates)
			{
				if (dateNode.ChildNodes.Count != 0)
				{
					MemberSpecialDate date = new MemberSpecialDate();
					XmlNode wrkNode = dateNode.SelectSingleNode("id");
					date.Id = wrkNode.InnerText;
					DateTime wrkDate;
					wrkNode = dateNode.SelectSingleNode("date");
					DateTime.TryParse(wrkNode.InnerText, out wrkDate);
					date.Date = wrkDate;
					wrkNode = dateNode.SelectSingleNode("date_changed");
					DateTime.TryParse(wrkNode.InnerText, out wrkDate);
					date.DateChanged = wrkDate;
					wrkNode = dateNode.SelectSingleNode("location");
					date.Location = wrkNode.InnerText;
					wrkNode = dateNode.SelectSingleNode("extra_1");
					date.Extra1 = wrkNode.InnerText;
					wrkNode = dateNode.SelectSingleNode("extra_2");
					date.Extra2 = wrkNode.InnerText;
					wrkNode = dateNode.SelectSingleNode("extra_3");
					date.Extra3 = wrkNode.InnerText;
					wrkNode = dateNode.SelectSingleNode("extra_4");
					date.Extra4 = wrkNode.InnerText;
					wrkNode = dateNode.SelectSingleNode("extra_5");
					date.Extra5 = wrkNode.InnerText;
					wrkNode = dateNode.SelectSingleNode("extra_6");
					date.Extra6 = wrkNode.InnerText;
					rtnDates.Add(date);					
				}
			}
			return rtnDates;
		}
		
		public IconAuth Auth
		{
			get { return auth; } 
			set { auth = value; }
		}
	}
	
	public class MemberEntry
	{
		public string Id { get; set; }
		public string HouseholdId { get; set; }
		public DateTime DateChanged { get; set; }
		public string Status { get; set; }
		public DateTime StatusDate { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string PreferredName { get; set; }
		public string MaidenName { get; set; }
		public string MailTo { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Phone { get; set; }
		public string Occupation { get; set; }
		public string WorkPlace { get; set; }
		public string WorkPhone { get; set; }
		public string WorkPhoneExtension { get; set; }
		public bool WorkPhoneUnlisted { get; set; }
		public bool Primary { get; set; }
		public string Gender { get; set; }
		public string Relationship { get; set; }
		public string PrimaryLanguage { get; set; }
		public string SecondaryLanguage { get; set; }
		public string Ethnicity { get; set; }
		public string Denomination { get; set; }
		public Collection<MemberPhone> Phones { get; set; }
		public Collection<MemberEmail> Emails { get; set; }
		public Collection<MemberSpecialDate> SpecialDates { get; set; }
		public string Picture { get; set; }
		public string Thumbnail { get; set; }
		public MemberEntry()
		{
			Id = string.Empty;
			HouseholdId = string.Empty;
			DateChanged = DateTime.MinValue;
			Status = string.Empty;
			StatusDate = DateTime.MinValue;
			Title = string.Empty;
			FirstName = string.Empty;
			MiddleName = string.Empty;
			LastName = string.Empty;
			PreferredName = string.Empty;
			MaidenName = string.Empty;
			MailTo = string.Empty;
			Address1 = string.Empty;
			Address2 = string.Empty;
			City = string.Empty;
			State = string.Empty;
			Zip = string.Empty;
			Phone = string.Empty;
			Occupation = string.Empty;
			WorkPlace = string.Empty;
			WorkPhone = string.Empty;
			WorkPhoneExtension = string.Empty;
			WorkPhoneUnlisted = false;
			Primary = false;
			Gender = string.Empty;
			Relationship = string.Empty;
			PrimaryLanguage = string.Empty;
			SecondaryLanguage = string.Empty;
			Ethnicity = string.Empty;
			Denomination = string.Empty;
			Phones = new Collection<MemberPhone>();
			Emails = new Collection<MemberEmail>();
			SpecialDates = new Collection<MemberSpecialDate>();
			Picture = string.Empty;
			Thumbnail = string.Empty;
		}
		
	}
	
	public class MemberPhone 
	{
		public string Id { get; set; }
		public string Phone { get; set; }
		public string Extension { get; set; }
		public string Provider { get; set; }
		public bool PhoneUnlisted { get; set; }
		public DateTime DateChanged { get; set; }
		
		public MemberPhone()
		{
			Id = string.Empty;
			Phone = string.Empty;
			Extension = string.Empty;
			Provider = string.Empty;
			PhoneUnlisted = false;
			DateChanged = DateTime.MinValue;
		}
	
	}
	
	public class MemberEmail
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public bool EmailUnlisted { get; set; }
		public DateTime DateChanged { get; set; }
		
		public MemberEmail()
		{
			Id = string.Empty;
			Email = string.Empty;
			EmailUnlisted = false;
		}
	}

	public class MemberSpecialDate
	{
		public string Id { get; set; }
		public DateTime Date { get; set; }
		public DateTime DateChanged { get; set; }
		public string Location { get; set; }
		public string Extra1 { get; set; }
		public string Extra2 { get; set; }
		public string Extra3 { get; set; }
		public string Extra4 { get; set; }
		public string Extra5 { get; set; }
		public string Extra6 { get; set; }		

		public MemberSpecialDate()
		{
			Id = string.Empty;
			Date = DateTime.MinValue;
			DateChanged = DateTime.MinValue;
			Location = string.Empty;
			Extra1 = string.Empty;
			Extra2 = string.Empty;
			Extra3 = string.Empty;
			Extra4 = string.Empty;
			Extra5 = string.Empty;
			Extra6 = string.Empty;			
		}
	}
			
	
}

