using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Implementations;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Tests
{
    [TestFixture]
    public class SupplierServiceTests
    {
        private ApplicationDbContext _context = null!;
        private SupplierService _service = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _service = new SupplierService(_context);
        }

        [TearDown]
        public void TearDown() => _context.Dispose();

        [Test]
        public async Task GetAllAsync_WithActiveOnly_ReturnsOnlyActiveSuppliers()
        {
            _context.Suppliers.AddRange(
                new Supplier { Name = "SupplierA", IsActive = true, Email = "a@test.com" },
                new Supplier { Name = "SupplierB", IsActive = false, Email = "b@test.com" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync(true);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("SupplierA"));
        }

        [Test]
        public async Task GetAllAsync_NoFilter_ReturnsAllSuppliers()
        {
            _context.Suppliers.AddRange(
                new Supplier { Name = "SupplierA", IsActive = true, Email = "a@test.com" },
                new Supplier { Name = "SupplierB", IsActive = false, Email = "b@test.com" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync();
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task CreateAsync_AddsSupplierToDatabase()
        {
            var model = new SupplierServiceModel
            {
                Name = "New Supplier",
                ContactPerson = "John",
                Email = "john@test.com",
                Phone = "0888123456",
                Address = "Sofia"
            };

            await _service.CreateAsync(model);

            Assert.That(await _context.Suppliers.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteAsync_SoftDeletesSupplier()
        {
            var supplier = new Supplier { Name = "Test", IsActive = true, Email = "t@test.com" };
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(supplier.Id);

            Assert.That(result, Is.True);
            var deleted = await _context.Suppliers.FindAsync(supplier.Id);
            Assert.That(deleted!.IsActive, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync(999);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsSupplier_WhenExists()
        {
            var supplier = new Supplier { Name = "Active", IsActive = true, Email = "act@test.com" };
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(supplier.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Active"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var result = await _service.GetByIdAsync(999);
            Assert.That(result, Is.Null);
        }
    }
}
