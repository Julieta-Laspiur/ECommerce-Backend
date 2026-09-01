using System.Net.Http.Json;
using System.Text.Json;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Payments.Dtos;

namespace ECommerce.Infrastructure.Services;

public class PaymentClient : IPaymentClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;

    public PaymentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "/api/payments/process",
            request,
            JsonOptions);

        response.EnsureSuccessStatusCode();

        var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponseDto>(JsonOptions);

        return paymentResponse
            ?? throw new InvalidOperationException("PaymentService returned an empty response.");
    }
}