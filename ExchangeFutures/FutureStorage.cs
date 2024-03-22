namespace ExchangeFutures
{
    public class FutureStorage
    {
        //словарь для хранения цен
        private readonly Dictionary<DateTime, double> priceData = new Dictionary<DateTime, double>();

        //максимальный срок хранения цены (24 часа)
        private readonly TimeSpan maxDataAge = TimeSpan.FromHours(24);

        public void AddPrice(DateTime timestamp, double price)
        {
            // Запись новых данных
            priceData.Add(timestamp, price);

            // Удаление старых данных
            CleanOldData();
        }

        //Получить минимальную и максимальную цену
        public (double, double) GetMinAndMaxPrice(DateTime from, DateTime to)
        {
            return (GetMinMaxPrice(from, to, true), GetMinMaxPrice(from, to, false));
        }

        //Получить минимальную цену
        public double GetMinPrice(DateTime from, DateTime to)
        {
            return GetMinMaxPrice(from, to, true);
        }

        //Получить максимальную цену
        public double GetMaxPrice(DateTime from, DateTime to)
        {
            return GetMinMaxPrice(from, to, false);
        }

        private double GetMinMaxPrice(DateTime from, DateTime to, bool isMin)
        {
            // Тут достаём цену за определённый промежуток времени
            var result = priceData
                .Where(item => item.Key >= from && item.Key <= to)
                .Select(item => item.Value);

            // Возврат минимальной или максимальной цены
            if (isMin)
                return result.DefaultIfEmpty(double.MaxValue).Min();
            else
                return result.DefaultIfEmpty(double.MinValue).Max();
        }

        private void CleanOldData()
        {
            // Очищение старых данных
            var now = DateTime.Now;
            var oldKeys = priceData.Keys.Where(key => now - key > maxDataAge).ToList();
            foreach (var key in oldKeys)
            {
                priceData.Remove(key);
            }
        }
    }
}
