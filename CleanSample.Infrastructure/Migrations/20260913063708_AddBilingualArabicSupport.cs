using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanSample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBilingualArabicSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VehicleOffloadStatuses",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "VehicleOffloadStatuses",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VehicleLoadStatuses",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "VehicleLoadStatuses",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "FullNameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Screens",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Screens",
                newName: "DescriptionEn");

            migrationBuilder.RenameIndex(
                name: "UQ_Screens_Name",
                table: "Screens",
                newName: "UQ_Screens_NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "NameEn");

            migrationBuilder.RenameIndex(
                name: "UQ_Roles_Name",
                table: "Roles",
                newName: "UQ_Roles_NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Parts",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Parts",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "OrderStatuses",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "OrderStatuses",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Materials",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Materials",
                newName: "DescriptionEn");

            migrationBuilder.RenameIndex(
                name: "UQ_Materials_Name",
                table: "Materials",
                newName: "UQ_Materials_NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "LoadRequestStatuses",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "LoadRequestStatuses",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "IssueStatuses",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "IssueStatuses",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FieldJobStatuses",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FieldJobStatuses",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FieldAssemblyStatuses",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FieldAssemblyStatuses",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Designs",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Designs",
                newName: "DescriptionEn");

            migrationBuilder.RenameIndex(
                name: "UQ_Designs_Name",
                table: "Designs",
                newName: "UQ_Designs_NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Colors",
                newName: "NameEn");

            migrationBuilder.RenameIndex(
                name: "UQ_Colors_Name",
                table: "Colors",
                newName: "UQ_Colors_NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "DescriptionEn");

            migrationBuilder.RenameIndex(
                name: "UQ_Categories_Name",
                table: "Categories",
                newName: "UQ_Categories_NameEn");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "VehicleOffloadStatuses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "VehicleOffloadStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "VehicleLoadStatuses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "VehicleLoadStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameAr",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Screens",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Screens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Roles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Roles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Roles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Parts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Parts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "OrderStatuses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "OrderStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Materials",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Materials",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "LoadRequestStatuses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "LoadRequestStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "IssueStatuses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "IssueStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "FieldJobStatuses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "FieldJobStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "FieldAssemblyStatuses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "FieldAssemblyStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Designs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Designs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Colors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Categories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FieldAssemblyStatuses",
                keyColumn: "FieldAssemblyStatusId",
                keyValue: 1,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "أعمال التركيب قيد التنفيذ", "قيد التنفيذ" });

            migrationBuilder.UpdateData(
                table: "FieldAssemblyStatuses",
                keyColumn: "FieldAssemblyStatusId",
                keyValue: 2,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم اكتمال أعمال التركيب", "مكتمل" });

            migrationBuilder.UpdateData(
                table: "FieldAssemblyStatuses",
                keyColumn: "FieldAssemblyStatusId",
                keyValue: 3,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إلغاء أعمال التركيب", "ملغي" });

            migrationBuilder.UpdateData(
                table: "FieldJobStatuses",
                keyColumn: "FieldJobStatusId",
                keyValue: 1,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تمت جدولة المهمة الميدانية", "مجدول" });

            migrationBuilder.UpdateData(
                table: "FieldJobStatuses",
                keyColumn: "FieldJobStatusId",
                keyValue: 2,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "المهمة الميدانية قيد التنفيذ", "قيد التنفيذ" });

            migrationBuilder.UpdateData(
                table: "FieldJobStatuses",
                keyColumn: "FieldJobStatusId",
                keyValue: 3,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم اكتمال المهمة الميدانية بنجاح", "مكتمل" });

            migrationBuilder.UpdateData(
                table: "FieldJobStatuses",
                keyColumn: "FieldJobStatusId",
                keyValue: 4,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إلغاء المهمة الميدانية", "ملغي" });

            migrationBuilder.UpdateData(
                table: "IssueStatuses",
                keyColumn: "IssueStatusId",
                keyValue: 1,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "المشكلة مفتوحة وغير محلولة", "مفتوح" });

            migrationBuilder.UpdateData(
                table: "IssueStatuses",
                keyColumn: "IssueStatusId",
                keyValue: 2,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "المشكلة قيد الفحص والحل", "قيد المعالجة" });

            migrationBuilder.UpdateData(
                table: "IssueStatuses",
                keyColumn: "IssueStatusId",
                keyValue: 3,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم حل المشكلة", "تم الحل" });

            migrationBuilder.UpdateData(
                table: "IssueStatuses",
                keyColumn: "IssueStatusId",
                keyValue: 4,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إغلاق المشكلة", "مغلق" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 1,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إنشاء طلب تحميل جديد", "جديد" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 2,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "اكتمل التحميل بحالة جيدة", "تم التحميل" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 3,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم تنزيل الشحنة بحالة جيدة في الوجهة", "تم التنزيل" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 4,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "اكتملت جميع أعمال التركيب الميداني", "مكتمل" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 5,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "معلق بسبب قطع تالفة أو مفقودة أثناء التحميل/التنزيل", "محظور / معلق" });

            migrationBuilder.UpdateData(
                table: "LoadRequestStatuses",
                keyColumn: "LoadRequestStatusId",
                keyValue: 6,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إلغاء طلب التحميل", "ملغي" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 1,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إنشاء أمر طلب مسودة", "مسودة" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 2,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "أمر الطلب بانتظار الموافقة", "قيد الانتظار" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 3,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم اعتماد أمر الطلب من مدير المبيعات", "معتمد" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 4,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إنشاء طلبات التحميل لأمر الطلب", "قيد المعالجة" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 5,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم تنفيذ جميع طلبات التحميل واكتمال أمر الطلب", "مكتمل" });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 6,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم إلغاء أمر الطلب", "ملغي" });

            migrationBuilder.UpdateData(
                table: "VehicleLoadStatuses",
                keyColumn: "VehicleLoadStatusId",
                keyValue: 1,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم التحميل بحالة سليمة وجيدة", "سليم" });

            migrationBuilder.UpdateData(
                table: "VehicleLoadStatuses",
                keyColumn: "VehicleLoadStatusId",
                keyValue: 2,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم التحميل مع وجود تلف", "تالف" });

            migrationBuilder.UpdateData(
                table: "VehicleLoadStatuses",
                keyColumn: "VehicleLoadStatusId",
                keyValue: 3,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "عناصر مفقودة أثناء التحميل", "مفقود" });

            migrationBuilder.UpdateData(
                table: "VehicleOffloadStatuses",
                keyColumn: "VehicleOffloadStatusId",
                keyValue: 1,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم التنزيل بحالة سليمة وجيدة", "سليم" });

            migrationBuilder.UpdateData(
                table: "VehicleOffloadStatuses",
                keyColumn: "VehicleOffloadStatusId",
                keyValue: 2,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "تم التنزيل مع وجود تلف", "تالف" });

            migrationBuilder.UpdateData(
                table: "VehicleOffloadStatuses",
                keyColumn: "VehicleOffloadStatusId",
                keyValue: 3,
                columns: new[] { "DescriptionAr", "NameAr" },
                values: new object[] { "عناصر مفقودة أثناء التنزيل", "مفقود" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "VehicleOffloadStatuses");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "VehicleOffloadStatuses");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "VehicleLoadStatuses");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "VehicleLoadStatuses");

            migrationBuilder.DropColumn(
                name: "FullNameAr",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "LoadRequestStatuses");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "LoadRequestStatuses");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "IssueStatuses");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "IssueStatuses");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "FieldJobStatuses");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "FieldJobStatuses");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "FieldAssemblyStatuses");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "FieldAssemblyStatuses");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Designs");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "VehicleOffloadStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "VehicleOffloadStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "VehicleLoadStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "VehicleLoadStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "FullNameEn",
                table: "Users",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Screens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Screens",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "UQ_Screens_NameEn",
                table: "Screens",
                newName: "UQ_Screens_Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "UQ_Roles_NameEn",
                table: "Roles",
                newName: "UQ_Roles_Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Parts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Parts",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "OrderStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "OrderStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Materials",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Materials",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "UQ_Materials_NameEn",
                table: "Materials",
                newName: "UQ_Materials_Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "LoadRequestStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "LoadRequestStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "IssueStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "IssueStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "FieldJobStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "FieldJobStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "FieldAssemblyStatuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "FieldAssemblyStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Designs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Designs",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "UQ_Designs_NameEn",
                table: "Designs",
                newName: "UQ_Designs_Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Colors",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "UQ_Colors_NameEn",
                table: "Colors",
                newName: "UQ_Colors_Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Categories",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "UQ_Categories_NameEn",
                table: "Categories",
                newName: "UQ_Categories_Name");
        }
    }
}
