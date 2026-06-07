using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Implementations;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Tests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private ApplicationDbContext _context = null!;
        private CategoryService _service = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _service = new CategoryService(_context);
        }

        [TearDown]
        public void TearDown() => _context.Dispose();

        [Test]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            _context.Categories.AddRange(
                new Category { Name = "Dairy", Description = "Dairy products" },
                new Category { Name = "Meat", Description = "Meat products" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync();
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectCategory()
        {
            var category = new Category { Name = "Beverages", Description = "Drinks" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(category.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Beverages"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var result = await _service.GetByIdAsync(999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_AddsCategoryToDatabase()
        {
            var model = new CategoryServiceModel { Name = "Snacks", Description = "Snack foods" };
            await _service.CreateAsync(model);

            Assert.That(await _context.Categories.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteAsync_RemovesCategory()
        {
            var category = new Category { Name = "Frozen", Description = "Frozen foods" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteAsync(category.Id);

            Assert.That(result, Is.True);
            Assert.That(await _context.Categories.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            var result = await _service.DeleteAsync(999);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ExistsAsync_ReturnsTrue_WhenCategoryExists()
        {
            var category = new Category { Name = "Bakery", Description = "Baked goods" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = await _service.ExistsAsync(category.Id);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ExistsAsync_ReturnsFalse_WhenNotFound()
        {
            var result = await _service.ExistsAsync(999);
            Assert.That(result, Is.False);
        }
    }
}
