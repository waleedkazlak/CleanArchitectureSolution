using System.Text.Json;
using System.Text.Json.Serialization;
using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.FieldAssembly;

[JsonConverter(typeof(CreateFieldAssemblyCommandJsonConverter))]
public class CreateFieldAssemblyCommand : IRequest<bool>
{
    public long? Id { get; set; }
    public long FieldJobId { get; set; }
    public int ProductId { get; set; }
    public string? ProductBarcode { get; set; }
    public int Quantity { get; set; }
    public DateTime? AssemblyDate { get; set; }
    public int Status { get; set; } = (int)Domain.Enums.FieldAssemblyStatusEnum.InProgress;
    public int? TechnicianId { get; set; }
    public int? SupervisorId { get; set; }
    public bool Verified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }

    public List<CreateFieldAssemblyItemDto> Items { get; set; } = new();

    public CreateFieldAssemblyCommand()
    {
    }

    public CreateFieldAssemblyCommand(List<CreateFieldAssemblyItemDto> items)
    {
        Items = items ?? new List<CreateFieldAssemblyItemDto>();
    }

    public CreateFieldAssemblyCommand(
        long fieldJobId,
        int productId,
        int quantity,
        long? id = null,
        string? productBarcode = null,
        DateTime? assemblyDate = null,
        int status = (int)Domain.Enums.FieldAssemblyStatusEnum.InProgress,
        int? technicianId = null,
        int? supervisorId = null,
        bool verified = false,
        DateTime? verifiedAt = null,
        string? notes = null)
    {
        Id = id;
        FieldJobId = fieldJobId;
        ProductId = productId;
        ProductBarcode = productBarcode;
        Quantity = quantity;
        AssemblyDate = assemblyDate;
        Status = status;
        TechnicianId = technicianId;
        SupervisorId = supervisorId;
        Verified = verified;
        VerifiedAt = verifiedAt;
        Notes = notes;
    }
}

public class CreateFieldAssemblyCommandJsonConverter : JsonConverter<CreateFieldAssemblyCommand>
{
    public override CreateFieldAssemblyCommand? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var items = JsonSerializer.Deserialize<List<CreateFieldAssemblyItemDto>>(ref reader, options) ?? new List<CreateFieldAssemblyItemDto>();
            return new CreateFieldAssemblyCommand(items);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if ((root.TryGetProperty("items", out var itemsElement) && itemsElement.ValueKind == JsonValueKind.Array) ||
                (root.TryGetProperty("Items", out itemsElement) && itemsElement.ValueKind == JsonValueKind.Array))
            {
                var items = JsonSerializer.Deserialize<List<CreateFieldAssemblyItemDto>>(itemsElement.GetRawText(), options) ?? new List<CreateFieldAssemblyItemDto>();
                return new CreateFieldAssemblyCommand(items);
            }

            var singleItem = JsonSerializer.Deserialize<CreateFieldAssemblyItemDto>(root.GetRawText(), options);
            if (singleItem != null)
            {
                return new CreateFieldAssemblyCommand(new List<CreateFieldAssemblyItemDto> { singleItem });
            }
        }

        return new CreateFieldAssemblyCommand();
    }

    public override void Write(Utf8JsonWriter writer, CreateFieldAssemblyCommand value, JsonSerializerOptions options)
    {
        if (value.Items != null && value.Items.Any())
        {
            JsonSerializer.Serialize(writer, value.Items, options);
        }
        else
        {
            writer.WriteStartObject();
            if (value.Id.HasValue) writer.WriteNumber("id", value.Id.Value);
            writer.WriteNumber("fieldJobId", value.FieldJobId);
            writer.WriteNumber("productId", value.ProductId);
            if (value.ProductBarcode != null) writer.WriteString("productBarcode", value.ProductBarcode);
            writer.WriteNumber("quantity", value.Quantity);
            if (value.AssemblyDate.HasValue) writer.WriteString("assemblyDate", value.AssemblyDate.Value.ToString("o"));
            writer.WriteNumber("status", value.Status);
            if (value.TechnicianId.HasValue) writer.WriteNumber("technicianId", value.TechnicianId.Value);
            if (value.SupervisorId.HasValue) writer.WriteNumber("supervisorId", value.SupervisorId.Value);
            writer.WriteBoolean("verified", value.Verified);
            if (value.VerifiedAt.HasValue) writer.WriteString("verifiedAt", value.VerifiedAt.Value.ToString("o"));
            if (value.Notes != null) writer.WriteString("notes", value.Notes);
            writer.WriteEndObject();
        }
    }
}
