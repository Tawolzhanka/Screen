# Screen Console Application

## Описание проекта
**Screen** — это консольное приложение на C#, разработанное для загрузки и обработки данных о заказах из XML-файла и сохранения их в базу данных SQLite. Программа считывает XML-файл с заказами, обрабатывает данные и сохраняет их в базе данных `Orders.db`, что позволяет использовать эту информацию для дальнейшего анализа.

## Основные возможности
- Загрузка данных из XML-файла, предоставленного внешним источником.
- Парсинг и валидация данных из XML-файла.
- Сохранение данных о заказах, продуктах и пользователях в базу данных SQLite (`Orders.db`).
- Поддержка unit-тестов для проверки функциональности репозиториев.

## Структура проекта

- **Screen**: Основное приложение.
  - `Program.cs`: основной файл, который запускает программу.
  - `Repositories`: папка с классами для работы с базой данных.
    - `IOrderDetailsRepository.cs`, `IOrderRepository.cs`, `IProductRepository.cs`, `IUserRepository.cs`: интерфейсы для работы с таблицами заказов, продуктов и пользователей.
    - `OrderDetailsRepository.cs`, `OrderRepository.cs`, `ProductRepository.cs`, `UserRepository.cs`: реализации репозиториев, которые взаимодействуют с базой данных `Orders.db`.

- **MyApp.Tests**: Проект для unit-тестирования.
  - `OrderRepositoryTests.cs`, `ProductRepositoryTests.cs`, `UserRepositoryTests.cs`: классы, покрывающие тестами функциональность репозиториев.

## Формат входного XML-файла
XML-файл `orders.xml` должен находиться в корневой папке проекта `Screen` и иметь следующую структуру:

```xml
<orders>
    <order>
        <no>1</no>
        <reg_date>2012.12.19</reg_date>
        <sum>234022.25</sum>
        <product>
            <quantity>2</quantity>
            <name>LG 1755</name>
            <price>12000.75</price>
        </product>
        <user>
            <fio>Иванов Иван Иванович</fio>
            <email>abc@email.com</email>
        </user>
    </order>
</orders>
```
## Требования
- .NET SDK: 7.0 или выше.
 - SQLite: База данных Orders.db.



