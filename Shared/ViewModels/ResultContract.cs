namespace DAL.Contracts
{
    /// <summary>
    ///شی قراردادی برای بازگشت دیتا
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultContract<T>
    {
        /// <summary>
        /// دیتای بازگشتی
        /// </summary>
        public T Data { get; set; }


        /// <summary>
        /// پیغام نمایشی
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public bool statuse { get; set; }


    }
}
