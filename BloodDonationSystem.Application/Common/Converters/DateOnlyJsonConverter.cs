using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BloodDonationSystem.Application.Common.Converters;

public class DateOnlyJsonConverter : JsonConverter<DateTime>
{
    private const string Format = "dd-MM-yyyy";

    // Este método é chamado ao transformar um objeto DateTime em uma string JSON (na resposta)
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }

    // Este método é chamado ao ler uma string JSON para transformar em um objeto DateTime (na requisição)
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Verifica se o valor lido é uma string
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("O valor da data deve ser uma string.");
        }

        var dateString = reader.GetString();

        // Tenta converter a string para DateTime usando o formato esperado.
        if (DateTime.TryParseExact(dateString, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }

        throw new JsonException(
            $"Não foi possível converter '{dateString}' para uma data. O formato esperado é '{Format}'.");
    }
}