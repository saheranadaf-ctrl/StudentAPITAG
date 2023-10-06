using CreateAPI.Controllers;
using CreateAPI.DataAccess;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory;

namespace StduentTest;
public class UnitTest1
{


        [Fact]
        public async Task CreateStudent_ValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new StudentDataContext(options))
            {
                var controller = new StudentController(dbContext);
                var student = new Student { StudentId = "STDN00481", Gender = "M" };

                // Act
                var result = await controller.CreateStudent(student);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var createdStudent = Assert.IsType<Student>(createdAtActionResult.Value);
                Assert.Equal(student.StudentId, createdStudent.StudentId);
            }
        }

        [Fact]
        public async Task CreateStudent_InvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new StudentDataContext(options))
            {
                var controller = new StudentController(dbContext);
                var student = new Student { StudentId = null, Gender = "M" }; // Invalid model

                // Act
                var result = await controller.CreateStudent(student);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }

        [Fact]
        public async Task DeleteStudent_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new StudentDataContext(options))
            {
                dbContext.Students.Add(new Student { StudentId = "STDN00490", Gender = "F" });
                dbContext.SaveChanges();
            }

            using (var dbContext = new StudentDataContext(options))
            {
                var controller = new StudentController(dbContext);

                // Act
                var result = await controller.DeleteStudent("STDN00490");

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var deletedStudent = Assert.IsType<Student>(okResult.Value);
                Assert.Equal("STDN00490", deletedStudent.StudentId);
            }
        }

        [Fact]
        public async Task DeleteStudent_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentDataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new StudentDataContext(options))
            {
                var controller = new StudentController(dbContext);

                // Act
                var result = await controller.DeleteStudent("NonExistingId");

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        [Fact]
        public async Task UpdateStudent_ValidId_ReturnsOkResult()
        {
            // Arrange
            var dbContextMock = new Mock<StudentDataContext>();
            var controller = new StudentController(dbContextMock.Object);
            var existingStudentId = "STDN123"; // Replace with an existing student ID
            var updatedStudent = new Student { StudentId = existingStudentId, Gender = "F" }; // Updated student with new gender

            dbContextMock.Setup(x => x.Students.FindAsync(existingStudentId))
                        .ReturnsAsync(new Student { StudentId = existingStudentId });

            // Act
            var result = await controller.UpdateStudent(existingStudentId, updatedStudent);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedStudentResult = Assert.IsType<Student>(okResult.Value);
            Assert.Equal(existingStudentId, updatedStudentResult.StudentId);
            Assert.Equal(updatedStudent.Gender, updatedStudentResult.Gender);
        }

        [Fact]
        public async Task UpdateStudent_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var dbContextMock = new Mock<StudentDataContext>();
            var controller = new StudentController(dbContextMock.Object);
            var nonExistingStudentId = "NONEXIST123"; // Replace with a non-existing student ID
            var updatedStudent = new Student { StudentId = nonExistingStudentId, Gender = "F" };

            dbContextMock.Setup(x => x.Students.FindAsync(nonExistingStudentId))
                        .ReturnsAsync((Student)null);

            // Act
            var result = await controller.UpdateStudent(nonExistingStudentId, updatedStudent);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateStudent_InvalidModelState_ReturnsBadRequestResult()
        {
            // Arrange
            var dbContextMock = new Mock<StudentDataContext>();
            var controller = new StudentController(dbContextMock.Object);
            var existingStudentId = "STDN123"; // Replace with an existing student ID
            var updatedStudent = new Student { StudentId = existingStudentId, Gender = null }; // Invalid model state

            // ModelState is invalid because 'Gender' is null

            // Act
            var result = await controller.UpdateStudent(existingStudentId, updatedStudent);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
}
