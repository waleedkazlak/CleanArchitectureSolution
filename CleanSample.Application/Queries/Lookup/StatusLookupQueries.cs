using CleanSample.Application.DTOs;
using CleanSample.Application.Helpers;
using CleanSample.Domain.Enums;
using MediatR;

namespace CleanSample.Application.Queries.Lookup;

public record GetAllStatusesLookupQuery : IRequest<AllStatusesLookupDto>;

public record GetOrderStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetLoadRequestStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetVehicleLoadStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetVehicleOffloadStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetFieldJobStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetFieldAssemblyStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetIssueStatusesLookupQuery : IRequest<List<StatusLookupDto>>;

public class StatusLookupQueryHandlers :
    IRequestHandler<GetAllStatusesLookupQuery, AllStatusesLookupDto>,
    IRequestHandler<GetOrderStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetLoadRequestStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetVehicleLoadStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetVehicleOffloadStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetFieldJobStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetFieldAssemblyStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetIssueStatusesLookupQuery, List<StatusLookupDto>>
{
    private static readonly Dictionary<OrderStatusEnum, (string NameEn, string NameAr, string DescEn, string DescAr)> OrderStatusMap = new()
    {
        { OrderStatusEnum.Draft, ("Draft", "مسودة", "Order created in draft status", "طلب تم إنشاؤه بحالة مسودة") },
        { OrderStatusEnum.Pending, ("Pending", "قيد الانتظار", "Order pending approval", "طلب بانتظار الاعتماد") },
        { OrderStatusEnum.Approved, ("Approved", "معتمد", "Order approved by sales manager", "طلب معتمد من مدير المبيعات") },
        { OrderStatusEnum.Processing, ("Processing", "قيد المعالجة", "Load requests placed for order", "تم إنشاء طلبات تحميل للطلب") },
        { OrderStatusEnum.Completed, ("Completed", "مكتمل", "All load requests fulfilled and order completed", "تم تلبية جميع طلبات التحميل واكتمال الطلب") },
        { OrderStatusEnum.Cancelled, ("Cancelled", "ملغي", "Order has been cancelled", "تم إلغاء الطلب") }
    };

    private static readonly Dictionary<LoadRequestStatusEnum, (string NameEn, string NameAr, string DescEn, string DescAr)> LoadRequestStatusMap = new()
    {
        { LoadRequestStatusEnum.New, ("New", "جديد", "Load request newly created", "طلب تحميل جديد") },
        { LoadRequestStatusEnum.Loaded, ("Loaded", "تم التحميل", "Loading completed in good condition", "اكتمل التحميل بحالة جيدة") },
        { LoadRequestStatusEnum.Offloaded, ("Offloaded", "تم التنزيل", "Offloaded in good condition at destination", "تم التنزيل بحالة جيدة في الوجهة") },
        { LoadRequestStatusEnum.Completed, ("Completed", "مكتمل", "All field assemblies completed", "اكتملت جميع أعمال التجميع الميداني") },
        { LoadRequestStatusEnum.Blocked, ("Blocked", "محظور", "Blocked due to damaged or missing parts during loading/offloading", "محظور لوجود قطع تالفة أو مفقودة أثناء التحميل/التنزيل") },
        { LoadRequestStatusEnum.Cancelled, ("Cancelled", "ملغي", "Load request has been cancelled", "تم إلغاء طلب التحميل") }
    };

    private static readonly Dictionary<VehicleLoadStatusEnum, (string NameEn, string NameAr, string DescEn, string DescAr)> VehicleLoadStatusMap = new()
    {
        { VehicleLoadStatusEnum.Good, ("Good", "سليم", "Loaded items in good condition", "العناصر المحملة بحالة سليمة") },
        { VehicleLoadStatusEnum.Damaged, ("Damaged", "تالف", "Loaded items with damage", "عناصر محملة بها تلف") },
        { VehicleLoadStatusEnum.Missing, ("Missing", "مفقود", "Expected items missing during load", "عناصر متوقعة مفقودة أثناء التحميل") }
    };

    private static readonly Dictionary<VehicleOffloadStatusEnum, (string NameEn, string NameAr, string DescEn, string DescAr)> VehicleOffloadStatusMap = new()
    {
        { VehicleOffloadStatusEnum.Good, ("Good", "سليم", "Offloaded items in good condition", "العناصر المنزلة بحالة سليمة") },
        { VehicleOffloadStatusEnum.Damaged, ("Damaged", "تالف", "Offloaded items with damage", "عناصر منزلة بها تلف") },
        { VehicleOffloadStatusEnum.Missing, ("Missing", "مفقود", "Expected items missing during offload", "عناصر متوقعة مفقودة أثناء التنزيل") }
    };

    private static readonly Dictionary<FieldJobStatusEnum, (string NameEn, string NameAr, string DescEn, string DescAr)> FieldJobStatusMap = new()
    {
        { FieldJobStatusEnum.Scheduled, ("Scheduled", "مجدول", "Field job scheduled for execution", "مهمة ميدانية مجدولة للتنفيذ") },
        { FieldJobStatusEnum.InProgress, ("InProgress", "قيد التنفيذ", "Field job currently in progress", "مهمة ميدانية قيد التنفيذ حالياً") },
        { FieldJobStatusEnum.Completed, ("Completed", "مكتمل", "Field job completed", "اكتملت المهمة الميدانية") },
        { FieldJobStatusEnum.Cancelled, ("Cancelled", "ملغي", "Field job cancelled", "تم إلغاء المهمة الميدانية") }
    };

    private static readonly Dictionary<FieldAssemblyStatusEnum, (string NameEn, string NameAr, string DescEn, string DescAr)> FieldAssemblyStatusMap = new()
    {
        { FieldAssemblyStatusEnum.InProgress, ("InProgress", "قيد التجميع", "Assembly currently in progress", "التجميع الميداني قيد التنفيذ حالياً") },
        { FieldAssemblyStatusEnum.Completed, ("Completed", "مكتمل", "Assembly successfully completed", "اكتمل التجميع بنجاح") },
        { FieldAssemblyStatusEnum.Cancelled, ("Cancelled", "ملغي", "Assembly cancelled", "تم إلغاء التجميع") }
    };

    private static readonly Dictionary<IssueStatusEnum, (string NameEn, string NameAr, string DescEn, string DescAr)> IssueStatusMap = new()
    {
        { IssueStatusEnum.Open, ("Open", "مفتوح", "Issue is open and pending review", "البلاغ مفتوح وبانتظار المراجعة") },
        { IssueStatusEnum.InProgress, ("InProgress", "قيد المعالجة", "Issue is actively being investigated/resolved", "البلاغ قيد المتابعة والمعالجة") },
        { IssueStatusEnum.Resolved, ("Resolved", "تم الحل", "Issue resolution has been provided", "تم تقديم حل للبلاغ") },
        { IssueStatusEnum.Closed, ("Closed", "مغلق", "Issue is verified and closed", "تم التحقق وإغلاق البلاغ") }
    };

    public Task<AllStatusesLookupDto> Handle(GetAllStatusesLookupQuery request, CancellationToken cancellationToken)
    {
        var result = new AllStatusesLookupDto
        {
            OrderStatuses = GetOrderStatuses(),
            LoadRequestStatuses = GetLoadRequestStatuses(),
            VehicleLoadStatuses = GetVehicleLoadStatuses(),
            VehicleOffloadStatuses = GetVehicleOffloadStatuses(),
            FieldJobStatuses = GetFieldJobStatuses(),
            FieldAssemblyStatuses = GetFieldAssemblyStatuses(),
            IssueStatuses = GetIssueStatuses()
        };

        return Task.FromResult(result);
    }

    public Task<List<StatusLookupDto>> Handle(GetOrderStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetOrderStatuses());

    public Task<List<StatusLookupDto>> Handle(GetLoadRequestStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetLoadRequestStatuses());

    public Task<List<StatusLookupDto>> Handle(GetVehicleLoadStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetVehicleLoadStatuses());

    public Task<List<StatusLookupDto>> Handle(GetVehicleOffloadStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetVehicleOffloadStatuses());

    public Task<List<StatusLookupDto>> Handle(GetFieldJobStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetFieldJobStatuses());

    public Task<List<StatusLookupDto>> Handle(GetFieldAssemblyStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetFieldAssemblyStatuses());

    public Task<List<StatusLookupDto>> Handle(GetIssueStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetIssueStatuses());

    private static List<StatusLookupDto> GetOrderStatuses() =>
        Enum.GetValues<OrderStatusEnum>()
            .Select(e =>
            {
                var (nameEn, nameAr, descEn, descAr) = OrderStatusMap.GetValueOrDefault(e, (e.ToString(), e.ToString(), e.ToString(), e.ToString()));
                return new StatusLookupDto
                {
                    Id = (int)e,
                    Name = LocalizationHelper.Localize(nameEn, nameAr) ?? nameEn,
                    Description = LocalizationHelper.Localize(descEn, descAr),
                    NameEn = nameEn,
                    NameAr = nameAr,
                    DescriptionEn = descEn,
                    DescriptionAr = descAr
                };
            }).ToList();

    private static List<StatusLookupDto> GetLoadRequestStatuses() =>
        Enum.GetValues<LoadRequestStatusEnum>()
            .Select(e =>
            {
                var (nameEn, nameAr, descEn, descAr) = LoadRequestStatusMap.GetValueOrDefault(e, (e.ToString(), e.ToString(), e.ToString(), e.ToString()));
                return new StatusLookupDto
                {
                    Id = (int)e,
                    Name = LocalizationHelper.Localize(nameEn, nameAr) ?? nameEn,
                    Description = LocalizationHelper.Localize(descEn, descAr),
                    NameEn = nameEn,
                    NameAr = nameAr,
                    DescriptionEn = descEn,
                    DescriptionAr = descAr
                };
            }).ToList();

    private static List<StatusLookupDto> GetVehicleLoadStatuses() =>
        Enum.GetValues<VehicleLoadStatusEnum>()
            .Select(e =>
            {
                var (nameEn, nameAr, descEn, descAr) = VehicleLoadStatusMap.GetValueOrDefault(e, (e.ToString(), e.ToString(), e.ToString(), e.ToString()));
                return new StatusLookupDto
                {
                    Id = (int)e,
                    Name = LocalizationHelper.Localize(nameEn, nameAr) ?? nameEn,
                    Description = LocalizationHelper.Localize(descEn, descAr),
                    NameEn = nameEn,
                    NameAr = nameAr,
                    DescriptionEn = descEn,
                    DescriptionAr = descAr
                };
            }).ToList();

    private static List<StatusLookupDto> GetVehicleOffloadStatuses() =>
        Enum.GetValues<VehicleOffloadStatusEnum>()
            .Select(e =>
            {
                var (nameEn, nameAr, descEn, descAr) = VehicleOffloadStatusMap.GetValueOrDefault(e, (e.ToString(), e.ToString(), e.ToString(), e.ToString()));
                return new StatusLookupDto
                {
                    Id = (int)e,
                    Name = LocalizationHelper.Localize(nameEn, nameAr) ?? nameEn,
                    Description = LocalizationHelper.Localize(descEn, descAr),
                    NameEn = nameEn,
                    NameAr = nameAr,
                    DescriptionEn = descEn,
                    DescriptionAr = descAr
                };
            }).ToList();

    private static List<StatusLookupDto> GetFieldJobStatuses() =>
        Enum.GetValues<FieldJobStatusEnum>()
            .Select(e =>
            {
                var (nameEn, nameAr, descEn, descAr) = FieldJobStatusMap.GetValueOrDefault(e, (e.ToString(), e.ToString(), e.ToString(), e.ToString()));
                return new StatusLookupDto
                {
                    Id = (int)e,
                    Name = LocalizationHelper.Localize(nameEn, nameAr) ?? nameEn,
                    Description = LocalizationHelper.Localize(descEn, descAr),
                    NameEn = nameEn,
                    NameAr = nameAr,
                    DescriptionEn = descEn,
                    DescriptionAr = descAr
                };
            }).ToList();

    private static List<StatusLookupDto> GetFieldAssemblyStatuses() =>
        Enum.GetValues<FieldAssemblyStatusEnum>()
            .Select(e =>
            {
                var (nameEn, nameAr, descEn, descAr) = FieldAssemblyStatusMap.GetValueOrDefault(e, (e.ToString(), e.ToString(), e.ToString(), e.ToString()));
                return new StatusLookupDto
                {
                    Id = (int)e,
                    Name = LocalizationHelper.Localize(nameEn, nameAr) ?? nameEn,
                    Description = LocalizationHelper.Localize(descEn, descAr),
                    NameEn = nameEn,
                    NameAr = nameAr,
                    DescriptionEn = descEn,
                    DescriptionAr = descAr
                };
            }).ToList();

    private static List<StatusLookupDto> GetIssueStatuses() =>
        Enum.GetValues<IssueStatusEnum>()
            .Select(e =>
            {
                var (nameEn, nameAr, descEn, descAr) = IssueStatusMap.GetValueOrDefault(e, (e.ToString(), e.ToString(), e.ToString(), e.ToString()));
                return new StatusLookupDto
                {
                    Id = (int)e,
                    Name = LocalizationHelper.Localize(nameEn, nameAr) ?? nameEn,
                    Description = LocalizationHelper.Localize(descEn, descAr),
                    NameEn = nameEn,
                    NameAr = nameAr,
                    DescriptionEn = descEn,
                    DescriptionAr = descAr
                };
            }).ToList();
}
