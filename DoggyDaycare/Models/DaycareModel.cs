using System;
namespace DoggyDaycare.Pages.Models
{
	public class DaycareModel
	{
		public int Id { get; set; }
		public string Owner { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public int Duration { get; set; }
	}
}

