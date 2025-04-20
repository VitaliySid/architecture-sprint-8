namespace report_api.Reports;

using System;

public record Report(
    int Id,
    string Title,
    DateTime CreatedAt,
    bool IsCompleted
);

public static class FakeReports
{
    public static Report[] Generate() =>
    [
        new Report(1, "Ежеквартальный финансовый отчет", new DateTime(2023, 5, 10, 14, 30, 0, DateTimeKind.Utc), true),
        new Report(2, "Анализ пользовательской активности за июнь", new DateTime(2023, 6, 15, 9, 45, 0, DateTimeKind.Utc), false),
        new Report(3, "Отчет по тестированию новой функциональности", new DateTime(2023, 7, 22, 16, 20, 0, DateTimeKind.Utc), true),
        new Report(4, "Итоги маркетинговой кампании Q3", new DateTime(2023, 8, 5, 11, 10, 0, DateTimeKind.Utc), false),
        new Report(5, "Аудит безопасности системы", new DateTime(2023, 9, 18, 13, 25, 0, DateTimeKind.Utc), true),
        new Report(6, "Отчет по обслуживанию серверов", new DateTime(2023, 10, 30, 10, 0, 0, DateTimeKind.Utc), false),
        new Report(7, "Анализ конкурентов за 2023 год", new DateTime(2023, 11, 12, 15, 40, 0, DateTimeKind.Utc), true),
        new Report(8, "Отчет по удовлетворенности клиентов", new DateTime(2023, 12, 1, 8, 15, 0, DateTimeKind.Utc), false),
        new Report(9, "Планы развития на 2024 год", new DateTime(2024, 1, 9, 12, 50, 0, DateTimeKind.Utc), true),
        new Report(10, "Предварительный отчет по продажам за январь", new DateTime(2024, 1, 25, 17, 30, 0, DateTimeKind.Utc), false)
    ];
}


