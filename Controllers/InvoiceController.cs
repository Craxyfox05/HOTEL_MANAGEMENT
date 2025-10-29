using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Models;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<IActionResult> Generate(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("Hotel Management System").FontSize(20).FontColor(Colors.Blue.Darken3);
                                column.Item().Text("Booking Invoice").FontSize(16).FontColor(Colors.Grey.Darken2);
                            });
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Customer Details:").Bold();
                                    c.Item().Text(booking.User.Name);
                                    c.Item().Text(booking.User.Email);
                                    c.Item().Text(booking.User.Phone ?? "");
                                });

                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Booking Details:").Bold();
                                    c.Item().Text($"Booking ID: {booking.Id}");
                                    c.Item().Text($"Booking Date: {booking.BookingDate:dd/MM/yyyy}");
                                    c.Item().Text($"Check-in: {booking.CheckIn:dd/MM/yyyy}");
                                    c.Item().Text($"Check-out: {booking.CheckOut:dd/MM/yyyy}");
                                });
                            });

                            column.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Room Details:").Bold();
                                    c.Item().Text($"Room No: {booking.Room.RoomNo}");
                                    c.Item().Text($"Type: {booking.Room.Type}");
                                    c.Item().Text($"Price per night: ₹{booking.Room.Price:N2}");
                                });
                            });

                            column.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Total Amount:");
                                row.ConstantItem(150).AlignRight().Text($"₹{booking.TotalAmount:N2}").Bold();
                            });

                            if (booking.Payment != null)
                            {
                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Payment Method: {booking.Payment.Method}");
                                    row.ConstantItem(150).AlignRight().Text($"Paid: ₹{booking.Payment.Amount:N2}").Bold();
                                });
                            }

                            column.Item().PaddingTop(20).Text("Thank you for choosing our hotel!").Italic().AlignCenter();
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", $"Invoice_{booking.Id}.pdf");
        }
    }
}

