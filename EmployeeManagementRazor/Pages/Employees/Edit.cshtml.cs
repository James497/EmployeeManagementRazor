using EmployeeManagementRazor.Models;
using EmployeeManagementRazor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazor.Pages.Employees
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditModel(IEmployeeRepository employeeRepository,
             IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [BindProperty]
        public Employee Employee { get; set; }

        public IActionResult OnGet(int? id)
        {
            if(id.HasValue)
            {
                Employee = _employeeRepository.GetEmployee(id.Value);
            }
            else
            {
                Employee = new Employee();
            }
            if (Employee == null)
            {
                return RedirectToPage("/NotFound");
            }
            return Page();
        }
        // We use this property to store and process
        // the newly uploaded photo
        [BindProperty]
        public IFormFile? Photo { get; set; }
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if (Photo != null)
            {
                // If a new photo is uploaded, the existing photo must be
                // deleted. So check if there is an existing photo and delete
                if (Employee.PhotoPath != null)
                {
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                        "images", Employee.PhotoPath);
                    if (Path.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                // Save the new photo in wwwroot/images folder and update
                // PhotoPath property of the employee object
                Employee.PhotoPath = ProcessUploadedFile();
            }
            if (Employee.Id > 0)
            {
                Employee = _employeeRepository.Update(Employee);
            }
            else
            {
                Employee = _employeeRepository.Add(Employee);
            }
            return RedirectToPage("/Index");
        }
        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        [BindProperty]
        public bool Notify { get; set; }

        public string Message { get; set; }
        public IActionResult OnPostUpdateNotificationPreferences(int id)
        {
            if (Notify)
            {
                Message = "Thank you for turning on notifications";
            }
            else
            {
                Message = "You have turned off email notifications";
            }

            TempData["message"] = Message;
            return RedirectToPage("Details", new { id = id });
        }

    }
}
