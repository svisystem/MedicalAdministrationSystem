using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Users
{
	public class SurgeryTimeM : NotifyPropertyChanged
	{
		private ObservableCollection<UserControl> _ExceptionsButton = new ObservableCollection<UserControl>();
		private ObservableCollection<Exception> _Exceptions = new ObservableCollection<Exception>();
		private List<int> _Erased = new List<int>();
		private bool _Monday;
		private bool _Tuesday;
		private bool _Wednesday;
		private bool _Thursday;
		private bool _Friday;
		private bool _Saturday;
		private bool _Sunday;
		private DateTime? _StartMonday;
		private DateTime? _FinishMonday;
		private DateTime? _StartTuesday;
		private DateTime? _FinishTuesday;
		private DateTime? _StartWednesday;
		private DateTime? _FinishWednesday;
		private DateTime? _StartThursday;
		private DateTime? _FinishThursday;
		private DateTime? _StartFriday;
		private DateTime? _FinishFriday;
		private DateTime? _StartSaturday;
		private DateTime? _FinishSaturday;
		private DateTime? _StartSunday;
		private DateTime? _FinishSunday;

		public ObservableCollection<UserControl> ExceptionsButton
		{
			get
			{
				return _ExceptionsButton;
			}
			set
			{
				if (_ExceptionsButton == value) return;
				_ExceptionsButton = value;
				OnPropertyChanged("ExceptionsButton");
			}
		}
		public ObservableCollection<Exception> Exceptions
		{
			get
			{
				return _Exceptions;
			}
			set
			{
				if (_Exceptions == value) return;
				_Exceptions = value;
				OnPropertyChanged("Exceptions");
			}
		}
		public List<int> Erased
		{
			get
			{
				return _Erased;
			}
			set
			{
				if (_Erased == value) return;
				_Erased = value;
				OnPropertyChanged("Erased");
			}
		}
		public bool Monday
		{
			get
			{
				return _Monday;
			}
			set
			{
				if (_Monday == value) return;
				_Monday = value;
				OnPropertyChanged("Monday");
			}
		}
		public bool Tuesday
		{
			get
			{
				return _Tuesday;
			}
			set
			{
				if (_Tuesday == value) return;
				_Tuesday = value;
				OnPropertyChanged("Tuesday");
			}
		}
		public bool Wednesday
		{
			get
			{
				return _Wednesday;
			}
			set
			{
				if (_Wednesday == value) return;
				_Wednesday = value;
				OnPropertyChanged("Wednesday");
			}
		}
		public bool Thursday
		{
			get
			{
				return _Thursday;
			}
			set
			{
				if (_Thursday == value) return;
				_Thursday = value;
				OnPropertyChanged("Thursday");
			}
		}
		public bool Friday
		{
			get
			{
				return _Friday;
			}
			set
			{
				if (_Friday == value) return;
				_Friday = value;
				OnPropertyChanged("Friday");
			}
		}
		public bool Saturday
		{
			get
			{
				return _Saturday;
			}
			set
			{
				if (_Saturday == value) return;
				_Saturday = value;
				OnPropertyChanged("Saturday");
			}
		}
		public bool Sunday
		{
			get
			{
				return _Sunday;
			}
			set
			{
				if (_Sunday == value) return;
				_Sunday = value;
				OnPropertyChanged("Sunday");
			}
		}
		public DateTime? StartMonday
		{
			get
			{
				return _StartMonday;
			}
			set
			{
				if (_StartMonday == value) return;
				_StartMonday = value;
				OnPropertyChanged("StartMonday");
			}
		}
		public DateTime? FinishMonday
		{
			get
			{
				return _FinishMonday;
			}
			set
			{
				if (_FinishMonday == value) return;
				_FinishMonday = value;
				OnPropertyChanged("FinishMonday");
			}
		}
		public DateTime? StartTuesday
		{
			get
			{
				return _StartTuesday;
			}
			set
			{
				if (_StartTuesday == value) return;
				_StartTuesday = value;
				OnPropertyChanged("StartTuesday");
			}
		}
		public DateTime? FinishTuesday
		{
			get
			{
				return _FinishTuesday;
			}
			set
			{
				if (_FinishTuesday == value) return;
				_FinishTuesday = value;
				OnPropertyChanged("FinishTuesday");
			}
		}
		public DateTime? StartWednesday
		{
			get
			{
				return _StartWednesday;
			}
			set
			{
				if (_StartWednesday == value) return;
				_StartWednesday = value;
				OnPropertyChanged("StartWednesday");
			}
		}
		public DateTime? FinishWednesday
		{
			get
			{
				return _FinishWednesday;
			}
			set
			{
				if (_FinishWednesday == value) return;
				_FinishWednesday = value;
				OnPropertyChanged("FinishWednesday");
			}
		}
		public DateTime? StartThursday
		{
			get
			{
				return _StartThursday;
			}
			set
			{
				if (_StartThursday == value) return;
				_StartThursday = value;
				OnPropertyChanged("StartThursday");
			}
		}
		public DateTime? FinishThursday
		{
			get
			{
				return _FinishThursday;
			}
			set
			{
				if (_FinishThursday == value) return;
				_FinishThursday = value;
				OnPropertyChanged("FinishThursday");
			}
		}
		public DateTime? StartFriday
		{
			get
			{
				return _StartFriday;
			}
			set
			{
				if (_StartFriday == value) return;
				_StartFriday = value;
				OnPropertyChanged("StartFriday");
			}
		}
		public DateTime? FinishFriday
		{
			get
			{
				return _FinishFriday;
			}
			set
			{
				if (_FinishFriday == value) return;
				_FinishFriday = value;
				OnPropertyChanged("FinishFriday");
			}
		}
		public DateTime? StartSaturday
		{
			get
			{
				return _StartSaturday;
			}
			set
			{
				if (_StartSaturday == value) return;
				_StartSaturday = value;
				OnPropertyChanged("StartSaturday");
			}
		}
		public DateTime? FinishSaturday
		{
			get
			{
				return _FinishSaturday;
			}
			set
			{
				if (_FinishSaturday == value) return;
				_FinishSaturday = value;
				OnPropertyChanged("FinishSaturday");
			}
		}
		public DateTime? StartSunday
		{
			get
			{
				return _StartSunday;
			}
			set
			{
				if (_StartSunday == value) return;
				_StartSunday = value;
				OnPropertyChanged("StartSunday");
			}
		}
		public DateTime? FinishSunday
		{
			get
			{
				return _FinishSunday;
			}
			set
			{
				if (_FinishSunday == value) return;
				_FinishSunday = value;
				OnPropertyChanged("FinishSunday");
			}
		}
		public class Exception : NotifyPropertyChanged
		{
			private int? _DBId;
			private int _Id;
			private bool _Included;
			private DateTime? _StartDateTime;
			private DateTime? _FinishDateTime;
			private bool _Valid;
			public int? DBId
			{
				get
				{
					return _DBId;
				}
				set
				{
					if (_DBId == value) return;
					_DBId = value;
					OnPropertyChanged("DBId");
				}
			}
			public int Id
			{
				get
				{
					return _Id;
				}
				set
				{
					if (_Id == value) return;
					_Id = value;
					OnPropertyChanged("Id");
				}
			}
			public bool Included
			{
				get
				{
					return _Included;
				}
				set
				{
					if (_Included == value) return;
					_Included = value;
					OnPropertyChanged("Included");
				}
			}
			public DateTime? StartDateTime
			{
				get
				{
					return _StartDateTime;
				}
				set
				{
					if (_StartDateTime == value) return;
					_StartDateTime = value;
					OnPropertyChanged("StartDateTime");
				}
			}
			public DateTime? FinishDateTime
			{
				get
				{
					return _FinishDateTime;
				}
				set
				{
					if (_FinishDateTime == value) return;
					_FinishDateTime = value;
					OnPropertyChanged("FinishDateTime");
				}
			}
			public bool Valid
			{
				get
				{
					return _Valid;
				}
				set
				{
					if (_Valid == value) return;
					_Valid = value;
					OnPropertyChanged("Valid");
				}
			}
		}
	}
}
