namespace StudentApplicationGuidance.ModelView
{
    public class UserSubjectSelectionModel
    {
        public List<UserSubjectModel> SelectedSubjects { get; set; } = new List<UserSubjectModel>();

        // Constructor to initialize the list with seven items
        public UserSubjectSelectionModel()
        {
            for (int i = 0; i < 7; i++)
            {
                // Initialize with a default model that could be bound to form elements
                SelectedSubjects.Add(new UserSubjectModel { Level = 1 }); // Defaulting level to 1 for safety
            }
        }
    }
}
