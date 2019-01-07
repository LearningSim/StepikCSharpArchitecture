using System;
using System.Linq;

namespace Incapsulation.EnterpriseTask {
	public class Enterprise {
		public Guid guid;
		public Guid Guid {
			get {
				return guid;
			}
		}

		public Enterprise(Guid guid) {
			this.guid = guid;
		}

		public string Name {
			get;
			set;
		}

		private string inn;
		public string Inn {
			get {
				return inn;
			}
			set {
				if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
					throw new ArgumentException();
				inn = value;
			}
		}

		public DateTime EstablishDate {
			get;
			set;
		}

		public TimeSpan ActiveTimeSpan {
			get {
				return DateTime.Now - EstablishDate;
			}
		}

		public double GetTotalTransactionsAmount() {
			DataBase.OpenConnection();
			var amount = 0.0;
			foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == guid)) {
				amount += t.Amount;
			}
			return amount;
		}
	}
}
