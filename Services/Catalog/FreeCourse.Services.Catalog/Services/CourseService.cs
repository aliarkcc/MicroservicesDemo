using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService: ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    item.Category = await _categoryCollection.Find(x => x.Id == item.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Successfull(_mapper.Map<List<CourseDto>>(courses), (int)HttpStatusCode.OK);
        }
        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (course != null)
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.Id).FirstAsync();
                return Response<CourseDto>.Successfull(_mapper.Map<CourseDto>(course), (int)HttpStatusCode.OK);
            }

            return Response<CourseDto>.Fail("Course Not Found!", (int)HttpStatusCode.NotFound);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var course = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

            if (course.Any())
            {
                foreach (var item in course)
                {
                    item.Category = await _categoryCollection.Find(x => x.Id == item.CategoryId).FirstAsync();
                }
            }
            else
            {
                return new Response<List<CourseDto>>();
            }

            return Response<List<CourseDto>>.Successfull(_mapper.Map<List<CourseDto>>(course), (int)HttpStatusCode.NotFound);
        }

        public async Task<Response<NoContent>> UpdateAsync(UpdateCourseDto updateCourseDto)
        {
            var course = _mapper.Map<Course>(updateCourseDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == updateCourseDto.Id, course);

            if (result is null)
            {
                return Response<NoContent>.Fail("Course not  found!",(int)HttpStatusCode.NotFound);
            }

            return Response<NoContent>.Successfull((int)HttpStatusCode.NoContent);
        }

        public async Task<Response<CourseDto>> AddAsync(AddCourseDto addCourseDto)
        {
            var course = _mapper.Map<Course>(addCourseDto);
            course.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(course);

            return Response<CourseDto>.Successfull(_mapper.Map<CourseDto>(course), (int)HttpStatusCode.OK);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result=await _courseCollection.DeleteOneAsync(x=>x.Id==id);

            if (result.DeletedCount>0)
            {
                return Response<NoContent>.Successfull((int)HttpStatusCode.NoContent);
            }
            return Response<NoContent>.Fail("Course not found!",(int)HttpStatusCode.NotFound);
        }
    }
}
