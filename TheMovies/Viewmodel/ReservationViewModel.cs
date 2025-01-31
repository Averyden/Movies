namespace TheMovies
{
    public class ReservationViewModel
    {
        public ReservationRepository Repository { get; set; }

        private Reservation reservation;

        public string CusName { get; set; }
        public string CusMail { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }
        public string[] Seat { get; set; }

        public Reservation Reserve => reservation;

        public ReservationViewModel(Reservation reservation)
        {
            this.reservation = reservation;
            Amount = reservation.Amount;
            Price = reservation.Price;
            Seat = reservation.Seat;
            
            string[] CusInfo = reservation.GetCustomerInfo().Split(';');

            CusName = CusInfo[0];
            CusMail = CusInfo[1];
        }

        public void DelReserve(int id)
        {
            Repository.Remove(id);
        }

    }
}
