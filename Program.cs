using System;
using System.Data.SQLite;
using System.Globalization;
using System.Xml.Linq;
using Screen.Repositories;

class Program
{
    static void Main(string[] args)
    {
        string xmlFilePath = "orders.xml";
        string dbFilePath = @"C:\Users\beshe\source\repos\Screen\Screen\Orders.db";

        using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
        {
            conn.Open();

            using (SQLiteTransaction transaction = conn.BeginTransaction()) // открытие транзакции
            {
                try
                {
                    XDocument xmlDoc = XDocument.Load(xmlFilePath);

                    foreach (var order in xmlDoc.Descendants("order"))
                    {
                        var userElement = order.Element("user");
                        string fio = userElement.Element("fio").Value;
                        string email = userElement.Element("email").Value;

                        int orderNo = int.Parse(order.Element("no").Value);
                        DateTime regDate = DateTime.ParseExact(order.Element("reg_date").Value, "yyyy.MM.dd", CultureInfo.InvariantCulture);
                        decimal totalAmount = decimal.Parse(order.Element("sum").Value, CultureInfo.InvariantCulture);

                        Console.WriteLine($"Обрабатываем заказ: no={orderNo}, date={regDate}, amount={totalAmount}");

                        IUserRepository userRepository = new UserRepository();
                        IOrderRepository orderRepository = new OrderRepository();
                        IProductRepository productRepository = new ProductRepository();
                        IOrderDetailsRepository orderDetailsRepository = new OrderDetailsRepository();

                        int userId = userRepository.GetOrInsertUser(conn, fio, email);
                        Console.WriteLine($"Пользователь добавлен или найден: {fio} (ID: {userId})");  // если пользователь не найден, то будет добавлен + логирование на данное действие

                        bool isOrderProcessed = orderRepository.IsOrderAlreadyProcessed(conn, orderNo);
                        if (isOrderProcessed)
                        {
                            Console.WriteLine($"Заказ {orderNo} уже был обработан. Пропуск.");
                            continue;
                        }

                        orderRepository.InsertOrUpdateOrder(conn, userId, regDate, totalAmount, orderNo);  // обновление или вставка заказа
                        Console.WriteLine($"Заказ {orderNo} добавлен или обновлен.");

                        foreach (var product in order.Elements("product"))
                        {
                            string productName = product.Element("name").Value;
                            int quantity = int.Parse(product.Element("quantity").Value);
                            decimal price = decimal.Parse(product.Element("price").Value, CultureInfo.InvariantCulture);

                            Console.WriteLine($"Обрабатываем продукт: {productName}, quantity={quantity}, price={price}");

                            int productId = productRepository.GetOrInsertProduct(conn, productName, price);
                            Console.WriteLine($"Продукт добавлен или найден: {productName} (ID: {productId})");

                            orderDetailsRepository.InsertOrUpdateOrderDetails(conn, orderNo, productId, quantity, price);
                            Console.WriteLine($"Детали заказа для {productName} добавлены или обновлены.");
                        }

                        orderRepository.MarkOrderAsProcessed(conn, orderNo);
                        Console.WriteLine($"Заказ {orderNo} отмечен как обработанный.");
                    }

                   
                    transaction.Commit(); // подтверждение транзакции
                    Console.WriteLine("Обработка завершена успешно.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // откат транзакции при ошибке
                    Console.WriteLine($"Ошибка при обработке: {ex.Message}");
                }
            }
        }
    }
}
