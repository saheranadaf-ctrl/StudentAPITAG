using CreateAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CreateAPI.Controllers
{

    
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {

        private readonly StudentDataContext _dbContext;
        private readonly ILogger<StudentController> _logger;
        private StudentDataContext dbContext;

        public StudentController(StudentDataContext dbContext,ILogger<StudentController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public StudentController(StudentDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

//--------------------------------- CREATE METHOD -------------------------------

        [HttpPost("Create")]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Log API request
                _logger.LogInformation($"Request received to create a new student: {JsonConvert.SerializeObject(student)}");

                
                // Add any additional data validation logic here

                _dbContext.Students.Add(student);
                 await _dbContext.SaveChangesAsync();

                // Log API response
                _logger.LogInformation($"Student created successfully: {JsonConvert.SerializeObject(student)}");


                return CreatedAtAction("GetStudentById", new { id = student.StudentId }, student);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                 
                _logger.LogError(ex, "An error occurred while creating a student.");

                //give the valid message with status code
                return StatusCode(500, "Internal server error");
            }
        }



//------------------------------ GET ALL METHOD ----------------------------------

        [HttpGet("GetAll")]
        public IActionResult GetAll(){
            var student = this._dbContext.Students.ToList();
            return Ok(student);
        }


//  --------------------------- UPDATE METHOD  -----------------------------------

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateStudent(string id, Student updatedStudent)
        {
            try
            {
                // Log API request
                _logger.LogInformation($"Request received to update a student: {JsonConvert.SerializeObject(updatedStudent)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingStudent = await _dbContext.Students.FirstOrDefaultAsync(s => s.StudentId == id);

                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                // Update the existing student properties with non-null values from updatedStudent
                existingStudent.Gender = updatedStudent.Gender ?? existingStudent.Gender;
                existingStudent.NationalIty = updatedStudent.NationalIty ?? existingStudent.NationalIty;
                existingStudent.PlaceofBirth = updatedStudent.PlaceofBirth ?? existingStudent.PlaceofBirth;
                existingStudent.StageId = updatedStudent.StageId ?? existingStudent.StageId;
                existingStudent.GradeId = updatedStudent.GradeId ?? existingStudent.GradeId;
                existingStudent.SectionId = updatedStudent.SectionId ?? existingStudent.SectionId;
                existingStudent.Topic = updatedStudent.Topic ?? existingStudent.Topic;
                existingStudent.Semester = updatedStudent.Semester ?? existingStudent.Semester;
                existingStudent.Relation = updatedStudent.Relation ?? existingStudent.Relation;
                existingStudent.Raisedhands = updatedStudent.Raisedhands ?? existingStudent.Raisedhands;
                existingStudent.VisItedResources = updatedStudent.VisItedResources ?? existingStudent.VisItedResources;
                existingStudent.AnnouncementsView = updatedStudent.AnnouncementsView ?? existingStudent.AnnouncementsView;
                existingStudent.Discussion = updatedStudent.Discussion ?? existingStudent.Discussion;
                existingStudent.ParentAnsweringSurvey = updatedStudent.ParentAnsweringSurvey ?? existingStudent.ParentAnsweringSurvey;
                existingStudent.ParentschoolSatisfaction = updatedStudent.ParentschoolSatisfaction ?? existingStudent.ParentschoolSatisfaction;
                existingStudent.StudentAbsenceDays = updatedStudent.StudentAbsenceDays ?? existingStudent.StudentAbsenceDays;
                existingStudent.StudentMarks = updatedStudent.StudentMarks ?? existingStudent.StudentMarks;
                existingStudent.Class = updatedStudent.Class ?? existingStudent.Class;

                _dbContext.Entry(existingStudent).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                 // Log API response
                _logger.LogInformation($"Student updated successfully: {JsonConvert.SerializeObject(updatedStudent)}");

                return Ok(existingStudent);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);

                _logger.LogError(ex, "An error occurred while updating a student.");

                //give the valid message with status code
                return StatusCode(500, "Internal server error");
            }
        }


// ---------------------------- DELETE METHOD ------------------------------

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                // Log API request
                _logger.LogInformation($"Request received to delete a student: {JsonConvert.SerializeObject(id)}");

                //check if the id is present
                var existingStudent = await _dbContext.Students.FindAsync(id);

                //if the entered is not found
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                //if the id entered is found
                _dbContext.Students.Remove(existingStudent);
                await _dbContext.SaveChangesAsync();

                 // Log API response
                _logger.LogInformation($"Student deleted successfully: {JsonConvert.SerializeObject(id)}");


                return Ok(existingStudent);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                //give the valid message with status code
                return StatusCode(500, "Internal server error");
            }
        }

// ---------------------------- GETBYID METHOD -----------------------------------
        
        [HttpGet("{id}")]
        public IActionResult GetStudentById(string id)
        {
            try
            {
                // Log API request
                _logger.LogInformation($"Request received to read a student: {JsonConvert.SerializeObject(id)}");

                var student = _dbContext.Students.FirstOrDefault(o=>o.StudentId == id);

                if (student == null)
                {
                    return NotFound("Student not found"); // Return 404 Not Found if the student with the given id is not found
                }

                 // Log API response
                _logger.LogInformation($"Student read Successfully: {JsonConvert.SerializeObject(id)}");


                return Ok(student); // Return 200 OK with the student data if found
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                 _logger.LogError(ex, "An error occurred while getting a student.");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
