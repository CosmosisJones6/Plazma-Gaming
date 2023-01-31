using ClientApp.RestClientLayer;
using System.ComponentModel;
using System.Data;
using ClientApp.ModelLayer;

namespace ClientApp.UILayer
{
    public partial class MainForm : Form
    {
        #region Fields
        private ApiDeveloperDataAccess developerApi;
        private ApiGameDataAccess gameApi;
        private ApiEventDataAccess eventApi;
        private ApiEventMemberDataAccess eventMemberApi;
        private ApiMemberDataAccess memberApi;
        private ApiSaleDataAccess saleApi;
        private ApiLoginDataAccess loginApi;
        private BindingSource salesSource;
        private byte[] CurrentGameFile { get; set; }//could be better solution
        public byte[] CurrentGameFileCreate { get; set; }//could be better solution
        public bool IsAdmin { get; set; }
        //creating and starting all the things in the constructor
        //also handling admin or non admin
        #endregion
        #region Constructor
        public MainForm(bool isAdmin)
        {
            IsAdmin = isAdmin;
            InitializeComponent();
            SetAccessAccordingToLogin();

            developerApi = new ApiDeveloperDataAccess("https://localhost:7023/api/v1/Developer");
            gameApi = new ApiGameDataAccess("https://localhost:7023/api/v1/Game");
            eventApi = new ApiEventDataAccess("https://localhost:7023/api/v1/Event");
            eventMemberApi = new ApiEventMemberDataAccess("https://localhost:7023/api/v1/eventmember");
            memberApi = new ApiMemberDataAccess("https://localhost:7023/api/v1/Member");
            gameApi = new ApiGameDataAccess("https://localhost:7023/api/v1/Game");
            saleApi = new ApiSaleDataAccess("https://localhost:7023/api/v1/Sale");
            loginApi = new ApiLoginDataAccess("https://localhost:7023/api/v1/Login");

            GetAllDevsFromApi();
            ClearDeveloperInputFields();
            ShowAllEvent();
            ClearEventInputFields();
            ShowAllGames();
            ClearGameUpdateInputFields();
            ClearGameCreateInputFields();
            GetAllSales();

            CurrentGameFile = null;
            CurrentGameFileCreate = null;

            panelCreateDeveloper.Visible = false;
            panelCreateEvent.Visible = false;
            panelCreateGame.Visible = false;
            labelCapacityCounter.Visible = false;
        }
        #endregion

        #region Helper Methods
        // helper method for admin non admin handling
        public void SetAccessAccordingToLogin()
        {
            if (IsAdmin == false)
            {
                panelAdminAccess.Visible = false;
                labelAuthorizationLoginPage.Visible = true;
            }
            else if (IsAdmin == true)
            {
                panelAdminAccess.Visible = true;
                labelAuthorizationLoginPage.Visible = false;
            }
        }

        public DialogResult GetDeleteMessageBoxResult()
        {
            DialogResult deleteResult = MessageBox.Show("Are you sure you want to delete this item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return deleteResult;
        }
        public DialogResult GetUpdateMessageBoxResult()
        {
            DialogResult deleteResult = MessageBox.Show("Are you sure you want to update this item? The old information will be lost.", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return deleteResult;
        }
        public void GetInputErrorMessageBox()
        {
            MessageBox.Show("You need to input all the necessary fields!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region Developer Page Wiring
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteDeveloper_Click(object sender, EventArgs e)
        {
            if (GetDeleteMessageBoxResult() == DialogResult.Yes)
            {
                DeleteDeveloper();
                ClearDeveloperInputFields();
                ShowAllDeveloper();
            }
        }

        private void buttonUpdateDeveloper_Click(object sender, EventArgs e)
        {
            if (CheckInputFieldsForUpdate() == true)
            {
                if (GetUpdateMessageBoxResult() == DialogResult.Yes)
                {
                    UpdateDeveloper();
                    ClearDeveloperInputFields();
                    ShowAllDeveloper();
                }
            }
            else
            {
                GetInputErrorMessageBox();
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonShowAllDeveloper_Click(object sender, EventArgs e)
        {
            ShowAllDeveloper();
            ClearDeveloperInputFields();
        }

        private void buttonSearchDeveloperByID_Click(object sender, EventArgs e)
        {
            ClearDeveloperInputFields();
            ChangeListToShowOnlyDeveloperBySearch();
            textBoxSearchDeveloper.Clear();
        }

        private void listBoxDeveloperList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDeveloperInputFields();

            Developer developerToShow = GetSelectedDeveloperObjectFromList();

            SetDeveloperInputFields(developerToShow);

            ShowAllGamesForDeveloper();


        }
        

        private void listBoxDeveloperGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearCreateDeveloperFormFields();
            panelCreateDeveloper.Visible = false;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonCreateDeveloper_Click(object sender, EventArgs e)
        {
            panelCreateDeveloper.Visible = true;
            ClearCreateDeveloperFormFields();
        }

        private void buttonFinishDeveloperCreation_Click(object sender, EventArgs e)
        {
            if (CheckInputFieldsForCreate() == true)
            {
                CreateNewDeveloper();
                ClearCreateDeveloperFormFields();
                panelCreateDeveloper.Visible = false;
                ShowAllDeveloper();
            }
            else
            {
                GetInputErrorMessageBox();
            }
            
        }

        private void listBoxDeveloperGames_DoubleClick(object sender, EventArgs e)
        {

        }
        private void listBoxDeveloperGames_MouseDoubleClick(object sender, EventArgs e)
        {
            

        }

        private void listBoxDeveloperGames_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RedirectToGamePage();
        }
        #endregion
        #region Developer Page Methods

        public void CreateNewDeveloper()
        {
            Developer dev = new Developer();
            dev.Name = textBoxCreateDeveloperName.Text;
            dev.Email = textBoxCreateDeveloperEmail.Text;
            dev.Description = textBoxCreateDeveloperDescription.Text;
            developerApi.CreateDeveloper(dev);
        }
        //when double click game in dev page we go to game page
        public void RedirectToGamePage()
        {
            string gameNameToSearch = listBoxDeveloperGames.Text;
            tabControl1.SelectedTab = GameMenuBar;
            listBoxGameList.SelectedItem = gameNameToSearch;
        }

        public void ClearCreateDeveloperFormFields()
        {
            textBoxCreateDeveloperName.Clear();
            textBoxCreateDeveloperEmail.Clear();
            textBoxCreateDeveloperDescription.Clear();
        }
        //we need to check if all the input fields has information so the creation runs smooth
        public bool CheckInputFieldsForCreate()
        {
            if (textBoxCreateDeveloperName.Text.Length > 0 && textBoxCreateDeveloperEmail.Text.Length > 0 && textBoxCreateDeveloperDescription.Text.Length > 0)
            {
                return true;
            }
            return false;
        }
        //we need to check if all the input fields has information so the updating runs smooth
        public bool CheckInputFieldsForUpdate()
        {
            if (textBoxDeveloperName.Text.Length > 0 && textBoxEmail.Text.Length > 0 && textBoxDeveloperDescription.Text.Length > 0)
            {
                return true;
            }
            return false;
        }
        //search by id 
        public void ChangeListToShowOnlyDeveloperBySearch()
        {
            Developer d = new Developer();
            try
            {
                d = developerApi.FindDeveloperFromId(int.Parse(textBoxSearchDeveloper.Text));
                listBoxDeveloperList.Items.Clear();
                listBoxDeveloperList.Items.Add(d.Name);
            }
            catch (Exception)
            {
                //error handling
            }
        }
        //populate list
        public void ShowAllDeveloper()
        {
            listBoxDeveloperList.Items.Clear();
            IEnumerable<Developer> developerList = developerApi.GetAllDevelopers();
            foreach (var developer in developerList)
            {
                listBoxDeveloperList.Items.Add(developer.Name);
            }
        }

        public void ShowAllGamesForDeveloper()
        {
            listBoxDeveloperGames.Items.Clear();
            IEnumerable<Game> gameList = gameApi.GetGamesByDeveloperId(GetSelectedDeveloperObjectFromList().DeveloperID);
            foreach (var game in gameList)
            {
                listBoxDeveloperGames.Items.Add(game.Title);
            }
        }

        public void DeleteDeveloper()
        {
            try
            {
                Developer developerToDelete = GetSelectedDeveloperObjectFromList();
                developerApi.DeleteDeveloper(developerToDelete);
                ShowAllDeveloper();

            }
            catch (Exception ex)
            {

                //error handling
            }
        }

        public void UpdateDeveloper()
        {
            Developer developerToUpdate = new Developer();
            developerToUpdate.DeveloperID = int.Parse(textBoxDeveloperID.Text);
            developerToUpdate.Name = textBoxDeveloperName.Text;
            developerToUpdate.Email = textBoxEmail.Text;
            developerToUpdate.Description = textBoxDeveloperDescription.Text;
            developerApi.UpdateDeveloper(developerToUpdate);
        }

        public IEnumerable<Developer> GetAllDevsFromApi()
        {
            listBoxDeveloperList.Items.Clear();
            IEnumerable<Developer> developerList = developerApi.GetAllDevelopers();
            foreach (var developer in developerList)
            {
                listBoxDeveloperList.Items.Add(developer.Name);
            }
            return developerList;
        }
        //we get the whole object from the list to show it in the input fields
        public Developer GetSelectedDeveloperObjectFromList()
        {
            Developer developerToShow = new Developer();
            IEnumerable<Developer> developerList = developerApi.GetAllDevelopers();
            foreach (Developer developer in developerList)
            {
                if (developer.Name.Equals(listBoxDeveloperList.SelectedItem))
                {
                    developerToShow = developer;
                }
            }
            return developerToShow;
        }

        public void ClearDeveloperInputFields()
        {
            textBoxDeveloperID.Clear();
            textBoxDeveloperName.Clear();
            textBoxEmail.Clear();
            textBoxDeveloperDescription.Clear();
            labelDeveloperNamePanel.ResetText();
            listBoxDeveloperGames.Items.Clear();
        }

        public void SetDeveloperInputFields(Developer developer)
        {
            textBoxDeveloperID.Text = developer.DeveloperID.ToString();
            textBoxDeveloperName.Text = developer.Name;
            textBoxEmail.Text = developer.Email;
            textBoxDeveloperDescription.Text = developer.Description;
            labelDeveloperNamePanel.Text = developer.Name;
        }
        #endregion


        #region Event Page Methods
        //populate list
        public void ShowAllEvent()
        {
            listBoxEvent.Items.Clear();
            IEnumerable<Event> eventList = eventApi.GetAllEvents();
            foreach (var e in eventList)
            {
                listBoxEvent.Items.Add(e.Name);
            }
        }

        public IEnumerable<int> GetAllMembersByEvent(int eventId)
        {
            try
            {
                IEnumerable<int> members = eventMemberApi.FindMembersFromEventId(eventId);
                return members;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
        //we need to get the whole object from list to show
        public Event GetSelectedEventObjectFromList()
        {
            Event eventToShow = new Event();
            IEnumerable<Event> eventList = eventApi.GetAllEvents();
            foreach (Event e in eventList)
            {
                if (e.Name.Equals(listBoxEvent.SelectedItem))
                {
                    eventToShow = e;
                }
            }
            return eventToShow;
        }
        //set the member list to the selected event
        public void SetSelectedMemberFromMemberToList()
        {
            int counter = 0;
            IEnumerable<Member> memberList = new List<Member>();

            IEnumerable<Member> allMemberObjects = new List<Member>();
            allMemberObjects = memberApi.GetAllMembers();

            IEnumerable<int> memberIdList = new List<int>();
            memberIdList = GetAllMembersByEvent(GetSelectedEventObjectFromList().EventID);

            foreach (int i in memberIdList)
            {
                foreach (Member m in allMemberObjects)
                {
                    if (i == m.MemberID)
                    {
                        listBoxEventMember.Items.Add(m.MemberID + " | " + m.Name + " | " + m.Email);
                        counter += 1;
                    }
                }
            }
            labelCapacityCounter.Text = counter.ToString() + "/" + numericUpDownEventPage.Value.ToString();
            labelCapacityCounter.Visible = true;
        }

        public void ClearEventInputFields()
        {
            textBoxEventId.Clear();
            textBoxEventGameId.Clear();
            textBoxEventName.Clear();
            textBoxEventDescription.Clear();
            labelEventNamePanel.ResetText();
            listBoxEventMember.Items.Clear();
            numericUpDownEventPage.ResetText();
            dateTimePickerEventStartDate.ResetText();
            dateTimePickerEventEndDate.ResetText();
            listBoxEventMember.Items.Clear();
            labelCapacityCounter.Visible = false;
        }

        public int GetSelectedMemberId()
        {
            string phrase = listBoxEventMember.SelectedItem.ToString();
            string[] words = phrase.Split(' ');

            return int.Parse(words[0]);
        }
        public void SetEventInputFields(Event e)
        {
            if (e.EventID != 0)
            {
                Game gameOfEvent = new Game();
                gameOfEvent = gameApi.GetGameUsingId(e.GameID);
                textBoxEventId.Text = e.EventID.ToString();
                textBoxEventGameId.Text = e.GameID.ToString() + " - " + gameOfEvent.Title;
                textBoxEventName.Text = e.Name;
                textBoxEventDescription.Text = e.Description;
                labelEventNamePanel.Text = e.Name;
                numericUpDownEventPage.Text = e.Capacity.ToString();
                dateTimePickerEventStartDate.Text = e.StartDate.ToString();
                dateTimePickerEventEndDate.Text = e.EndDate.ToString();
                SetSelectedMemberFromMemberToList();
            } 
        }

        public void SetCreateEventGameIdDropDown()
        {
            IEnumerable<Game> allGames = new List<Game>();
            allGames = gameApi.GetAllGames();
            foreach (Game g in allGames)
            {
                comboBoxCreateEventGameId.Items.Add(g.GameID + " - " + g.Title);
            }
        }

        public void UpdateEvent()
        {
            Event eventToUpdate = new Event();
            eventToUpdate.Name = textBoxEventName.Text;
            eventToUpdate.Description = textBoxEventDescription.Text;
            eventToUpdate.StartDate = dateTimePickerEventStartDate.Value.AddHours(1) ;
            eventToUpdate.EndDate = dateTimePickerEventEndDate.Value.AddHours(1);
            eventToUpdate.EventID = int.Parse(textBoxEventId.Text);
            eventApi.UpdateEvent(eventToUpdate);
        }

        public void RemoveMemberFromEvent(int eventId, int memberId)
        {
            EventMember em = new EventMember();
            em.EventID = eventId;
            em.MemberID = memberId;
            eventMemberApi.RemoveMemberFromEvent(em);
        }

        public void DeleteEvent()
        {
            try
            {
                Event eventToDelete = GetSelectedEventObjectFromList();
                eventApi.DeleteEvent(eventToDelete);

            }
            catch (Exception ex)
            {

                //error handling
            }
        }

        public bool CreateNewEvent()
        {   
            if (comboBoxCreateEventGameId.Text.Length > 0 && textBoxCreateEventName.Text.Length > 0 && textBoxCreateEventDescription.Text.Length > 0 && numericUpDownCreateEventCapacity.Text.Length > 0 && dateTimePickerCreateEventStartDate.Text.Length > 0 && dateTimePickerCreateEventEndDate.Text.Length > 0)
            {
                string[] wordsinString = comboBoxCreateEventGameId.Text.Split(" ");
                Event e = new Event();
                e.GameID = int.Parse(wordsinString[0]);
                e.Name = textBoxCreateEventName.Text;
                e.Description = textBoxCreateEventDescription.Text;
                e.Capacity = int.Parse(numericUpDownCreateEventCapacity.Text);
                e.StartDate = DateTime.Parse(dateTimePickerCreateEventStartDate.Text).AddHours(1);
                e.EndDate = DateTime.Parse(dateTimePickerCreateEventEndDate.Text).AddHours(1);
                try
                {
                    eventApi.CreateEvent(e);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                
                return true;
            }
            GetInputErrorMessageBox();
            return false;
        }

        public void ClearEventCreateFields()
        {
            numericUpDownCreateEventCapacity.ResetText();
            textBoxCreateEventName.Clear();
            comboBoxCreateEventGameId.ResetText();
            comboBoxCreateEventGameId.Items.Clear();
            textBoxCreateEventDescription.Clear();
            dateTimePickerCreateEventStartDate.ResetText();
            dateTimePickerCreateEventEndDate.ResetText();
        }
        public void FindEventFromId()
        {
            Event e = new Event();
            try
            {
                e = eventApi.FindEventFromId(int.Parse(textBoxEventIdSerach.Text));
                listBoxEvent.Items.Clear();
                listBoxEvent.Items.Add(e.Name);
            }
            catch (Exception)
            {
                //error handling
            }
        }

        #endregion
        #region Event Page Wiring
        private void buttonShowAllEvent_Click(object sender, EventArgs e)
        {
            textBoxEventIdSerach.Clear();
            ClearEventInputFields();
            ShowAllEvent();
        }

        private void listBoxEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearEventInputFields();
            SetEventInputFields(GetSelectedEventObjectFromList());
        }

        private void buttonUpdateEvent_Click(object sender, EventArgs e)
        {
            if (textBoxEventDescription.Text.Length > 0 && textBoxEventName.Text.Length > 0)
            {
                if (GetUpdateMessageBoxResult() == DialogResult.Yes)
                {
                    UpdateEvent();
                    ClearEventInputFields();
                    ShowAllEvent();
                }
            }
            else
            {
                GetInputErrorMessageBox();
            }
            
        }

        private void buttonDeleteEvent_Click(object sender, EventArgs e)
        {
            if (GetDeleteMessageBoxResult() == DialogResult.Yes)
            {
                DeleteEvent();
                ClearEventInputFields();
                ShowAllEvent();
            }
        }

        private void buttonCreateEventFinish_Click(object sender, EventArgs e)
        {
            bool success = CreateNewEvent();
            if (success == true)
            {
                panelCreateEvent.Visible = false;
            }
            ClearEventCreateFields();
            ShowAllEvent();
        }

        private void buttonCreateNewEvent_Click(object sender, EventArgs e)
        {
            panelCreateEvent.Visible = true;
            ClearEventCreateFields();
            SetCreateEventGameIdDropDown();
        }

        private void buttonCreateEventCancel_Click(object sender, EventArgs e)
        {
            panelCreateEvent.Visible = false;
            ClearEventCreateFields();
        }

        private void labelEventendDateHour_Click(object sender, EventArgs e)
        {

        }

        private void EventMenuBar_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearchEventById_Click(object sender, EventArgs e)
        {
            ClearEventInputFields();
            FindEventFromId();
        }

        private void buttonDeleteMember_Click(object sender, EventArgs e)
        {
            if (listBoxEventMember.SelectedItem != null)
            {
                RemoveMemberFromEvent(GetSelectedEventObjectFromList().EventID, GetSelectedMemberId());
                listBoxEventMember.Items.Clear();
                SetSelectedMemberFromMemberToList();
            } 
        }

        private void listBoxEventMember_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Game Page Methods
        //populate game list
        public void ShowAllGames()
        {
            listBoxGameList.Items.Clear();
            IEnumerable<Game> gameList = gameApi.GetAllGames();
            foreach (var g in gameList)
            {
                listBoxGameList.Items.Add(g.Title);
            }
        }
        //get whole object from list to show
        public Game GetSelectedGameObjectFromList()
        {
            Game gameToShow = new Game();
            if (listBoxGameList.SelectedItem != null)
            {
               
                IEnumerable<Game> gameList = gameApi.GetAllGames();
                foreach (Game g in gameList)
                {
                    if (g.Title.Equals(listBoxGameList.SelectedItem))
                    {
                        gameToShow = g;
                        Game gameFile = new Game();
                        gameFile = GetSelectedGameFileObject(gameToShow.GameID);
                        gameToShow.FileName = gameFile.FileName;
                        gameToShow.FileContent = gameFile.FileContent;
                    }
                }  
            }
            return gameToShow;
        }
        //redirect to game page from dev page helper
        public Game GetSelectedGameObjectFromDeveloperGameList()
        {
            Game game = new Game();
            IEnumerable<Game> gameList = gameApi.GetAllGames();
            foreach (Game g in gameList)
            {
                if (g.Title.Equals(listBoxDeveloperGames.SelectedItem))
                {
                    game = g;
                }
            }
            return game;
        }

        public void SetGameUpdateInputFields(Game game)
        {
            
            Developer gameDeveloper = new Developer();
            gameDeveloper = developerApi.FindDeveloperFromId(game.DeveloperID);
            textBoxUpdateGameGameId.Text = game.GameID.ToString();
            textBoxUpdateGameTitle.Text = game.Title;
            textBoxUpdateGameDeveloperId.Text = game.DeveloperID.ToString() + " - " + gameDeveloper.Name;
            textBoxUpdateGameType.Text = game.Type;
            textBoxUpdateGameDescription.Text = game.Description;
            numericUpDownUpdateGamePrice.Text = game.Price.ToString(); ;
            labelUpdateGameFileName.Text = game.FileName;
            numericUpDownUpdateGameYearOfRelease.Text = game.YearOfRelease.ToString();
            textBoxUpdateGameSpecifications.Text = game.Specifications;
            labelUpdateGameName.Text = game.Title;
            CurrentGameFile = game.FileContent;
        }

        public void SetCreateDeveloperListDropDown()
        {
            IEnumerable<Developer> allDev = developerApi.GetAllDevelopers();
            foreach (Developer d in allDev)
            {
                comboBoxCreateGameDeveloperList.Items.Add(d.DeveloperID + " - " + d.Name);
            }
            
        }

        public void ClearGameUpdateInputFields()
        {
            textBoxUpdateGameGameId.Clear();
            textBoxUpdateGameTitle.Clear();
            textBoxUpdateGameDeveloperId.Clear();
            textBoxUpdateGameType.Clear();
            textBoxUpdateGameDescription.Clear();
            numericUpDownUpdateGamePrice.ResetText();
            labelUpdateGameFileName.Text = "No File";
            numericUpDownUpdateGameYearOfRelease.ResetText();
            textBoxUpdateGameSpecifications.Clear();
            labelUpdateGameName.ResetText();
            CurrentGameFile = null;

        }

        public Game GetSelectedGameFileObject(int gameId)
        {
            Game gameFileToShow = gameApi.GetGameFileByGameId(gameId);
            return gameFileToShow;
        }

        public void UpdateGame()
        {
            if (AllUpdateFieldsAreCorrect() == true)
            {
                string[] wordsOfString = textBoxUpdateGameDeveloperId.Text.Split(" ");
                Game gameToUpdate = new Game();
                gameToUpdate.GameID = int.Parse(textBoxUpdateGameGameId.Text);
                gameToUpdate.Title = textBoxUpdateGameTitle.Text;
                gameToUpdate.Type = textBoxUpdateGameType.Text;
                gameToUpdate.Description = textBoxUpdateGameDescription.Text;
                gameToUpdate.DeveloperID = int.Parse(wordsOfString[0]);
                gameToUpdate.Price = float.Parse(numericUpDownUpdateGamePrice.Text);
                gameToUpdate.Specifications = textBoxUpdateGameSpecifications.Text;
                gameToUpdate.YearOfRelease = int.Parse(numericUpDownUpdateGameYearOfRelease.Text);
                gameToUpdate.FileName = labelUpdateGameFileName.Text;
                gameToUpdate.FileContent = CurrentGameFile;
                gameApi.UpdateGame(gameToUpdate);
            }
            else
            {
                GetInputErrorMessageBox();
            }

        }

        public bool AllUpdateFieldsAreCorrect()
        {
            if (textBoxUpdateGameTitle.Text.Length > 0 && textBoxUpdateGameDescription.Text.Length > 0 && numericUpDownUpdateGamePrice.Text.Length > 0 && textBoxUpdateGameDeveloperId.Text.Length > 0 && textBoxUpdateGameSpecifications.Text.Length > 0 && numericUpDownUpdateGameYearOfRelease.Text.Length > 0 && labelUpdateGameFileName.Text.Length > 0 && CurrentGameFile != null)
            {
                return true;
            }
            return false;
        }
        //file dialog pop up when button pressed
        public void SelectFileDialog()
        {
            DialogResult dialog = openFileDialogUpdateGame.ShowDialog();
            openFileDialogUpdateGame.Title = "Choose new game file for updating";
            string path = openFileDialogUpdateGame.FileName;

            if (dialog == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(path))
                {
                    //error
                }
                labelUpdateGameFileName.Text = Path.GetFileName(path);
                CurrentGameFile = File.ReadAllBytes(Path.GetFullPath(path));
            }
            if (dialog == DialogResult.Cancel)
            {
                if (string.IsNullOrEmpty(path))
                {
                    //error
                }
            }
        }
        //find one game by id and show in list
        public void FindGameFromId()
        {
            Game gameSearch = new Game();
            try
            {
                IEnumerable<Game> allGames = new List<Game>();
                allGames = gameApi.GetAllGames();
                foreach (Game game in allGames)
                {
                    if (game.GameID == int.Parse(textBoxSearchGame.Text))
                    {
                        gameSearch = game;
                    }
                }
                listBoxGameList.Items.Clear();
                listBoxGameList.Items.Add(gameSearch.Title);
            }
            catch (Exception)
            {
                //error handling
            }
        }

        //create game
        public void AddNewGame()
        {
            if (AllCreateFieldsAreCorrect() == true)
            {
                string[] wordsOfString = comboBoxCreateGameDeveloperList.Text.Split(" ");
                Game gameToCreate = new Game();
                gameToCreate.Title = textBoxCreateGameTitle.Text;
                gameToCreate.DeveloperID = int.Parse(wordsOfString[0]);
                gameToCreate.Type = textBoxCreateGameType.Text;
                gameToCreate.Description = textBoxCreateGameDescription.Text;
                gameToCreate.Specifications = textBoxCreateGameSpecifications.Text;
                gameToCreate.Price = float.Parse(numericUpDownCreateGamePrice.Text);
                gameToCreate.YearOfRelease = int.Parse(numericUpDownCreateGameYearOfRelease.Text);
                gameToCreate.FileName = labelCreateGameFileName.Text;
                gameToCreate.FileContent = CurrentGameFileCreate;
                gameApi.CreateGame(gameToCreate);
            }
            else
            {
                GetInputErrorMessageBox();
            }
        }

        public bool AllCreateFieldsAreCorrect()
        {
            if (CurrentGameFileCreate != null && textBoxCreateGameTitle.Text.Length > 0 && comboBoxCreateGameDeveloperList.Text.Length > 0 && textBoxCreateGameType.Text.Length > 0 && textBoxCreateGameDescription.Text.Length > 0 && textBoxCreateGameSpecifications.Text.Length > 0 && numericUpDownCreateGamePrice.Text.Length > 0 && numericUpDownCreateGameYearOfRelease.Text.Length > 0)
            {
                return true;
            }
            return false;
        }

        public void ClearGameCreateInputFields()
        {
            textBoxCreateGameTitle.Clear();
            comboBoxCreateGameDeveloperList.ResetText();
            textBoxCreateGameType.Clear();
            textBoxCreateGameDescription.Clear();
            numericUpDownCreateGamePrice.ResetText();
            labelCreateGameFileName.Text = "No File";
            numericUpDownCreateGameYearOfRelease.ResetText();
            textBoxCreateGameSpecifications.Clear();
            CurrentGameFileCreate = null;
        }
        // file dialog for create new game
        public void SelectFileDialogCreate()
        {
            DialogResult dialog = openFileDialogCreateGame.ShowDialog();
            openFileDialogCreateGame.Title = "Choose new game file";
            string path = openFileDialogCreateGame.FileName;

            if (dialog == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(path))
                {
                    //error
                }
                labelCreateGameFileName.Text = Path.GetFileName(path);
                CurrentGameFileCreate = File.ReadAllBytes(Path.GetFullPath(path));
            }
            if (dialog == DialogResult.Cancel)
            {
                if (string.IsNullOrEmpty(path))
                {
                    //error
                }
            }
        }

        public void DeleteGame()
        {
            gameApi.DeleteGame(GetSelectedGameObjectFromList().GameID);
        }
        #endregion
        #region Game Page Wiring
        private void listBoxGameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGameUpdateInputFields();
            SetGameUpdateInputFields(GetSelectedGameObjectFromList());
        }

        private void buttonUpdateGameChangeFile_Click(object sender, EventArgs e)
        {
            SelectFileDialog();
        }

        private void buttonUpdateGame_Click(object sender, EventArgs e)
        {
            if (GetUpdateMessageBoxResult() == DialogResult.Yes)
            {
                UpdateGame();
                ClearGameUpdateInputFields();
                ShowAllGames();
            }
        }

        private void buttonCreateNewGame_Click(object sender, EventArgs e)
        {
            comboBoxCreateGameDeveloperList.Items.Clear();
            panelCreateGame.Visible = true;
            SetCreateDeveloperListDropDown();
        }

        private void buttonCreateGameCancel_Click(object sender, EventArgs e)
        {
            ClearGameCreateInputFields();
            panelCreateGame.Visible = false;
        }

        private void buttonCreateGameFinish_Click(object sender, EventArgs e)
        {
            AddNewGame();
            ClearGameCreateInputFields();
            panelCreateGame.Visible = false;
            ShowAllGames();
        }

        private void buttonCreateGameFile_Click(object sender, EventArgs e)
        {
            SelectFileDialogCreate();
        }

        private void buttonShowAllGame_Click(object sender, EventArgs e)
        {
            ClearGameUpdateInputFields();
            ShowAllGames();
        }

        private void buttonDeleteGame_Click(object sender, EventArgs e)
        {
            if (GetDeleteMessageBoxResult() == DialogResult.Yes)
            {
                DeleteGame();
                ClearGameUpdateInputFields();
                ShowAllGames();
            }
        }

        private void buttonSearchGame_Click(object sender, EventArgs e)
        {
            FindGameFromId();
            ClearGameUpdateInputFields();
            textBoxSearchGame.ResetText();
        }
        #endregion

        #region Sale Page Methods

        //create a data table with the info from api, add it to the binding source, than add it to the advanced data grid view
        public void GetAllSales()
        {
            List<Sale> allSales = new List<Sale>();
            allSales = saleApi.GetAllSales();

            DataTable data = new DataTable();
            data.Columns.Add("GameKey");
            data.Columns.Add("GameID");
            data.Columns.Add("Email");
            data.Columns.Add("Date");
            data.Columns.Add("SalesPrice");

            foreach (Sale sale in allSales)
            {
                DataRow row = data.NewRow();
                row["GameKey"] = sale.GameKey;
                row["GameID"] = sale.GameID;
                row["Email"] = sale.Email;
                row["Date"] = sale.Date;
                row["SalesPrice"] = sale.SalesPrice;
                data.Rows.Add(row);
            }

            salesSource = new BindingSource();
            salesSource.DataSource = data;
            advancedDataGridViewSale.DataSource = salesSource;
            for (int i = 0; i < 5; i++)
            {
                advancedDataGridViewSale.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        public void ClearSaleTable()
        {
            salesSource.RemoveSort();
            salesSource.RemoveFilter();
            salesSource.DataSource = null;
        }
        #endregion
        #region Sale Page Wiring
        private void advancedDataGridViewSale_SortStringChanged(object sender, EventArgs e)
        {
            //sorting handle
            salesSource.Sort = advancedDataGridViewSale.SortString;
        }

        private void advancedDataGridViewSale_FilterStringChanged(object sender, EventArgs e)
        {
            //filter handle
            salesSource.Filter = advancedDataGridViewSale.FilterString;
        }

        private void salesSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            
        }

        private void buttonRefreshSale_Click_1(object sender, EventArgs e)
        {
            ClearSaleTable();
            GetAllSales();
        }
        #endregion

        #region Login Page Methods

        /*
          How login management works:
          - Only people with a certain login can delete and create logins (admins)
          - People with the non-admin accounts cannot make changes in this page
          - When we have a new employee and they need access to the system the admins will create new credentials for them
        */
        //populate login list
        public void GetAllLogins()
        {
            IEnumerable<Login> allLogins = new List<Login>();
            allLogins = loginApi.GetAllLogins();
            foreach (Login login in allLogins)
            {
                listBoxLogins.Items.Add(login.UserName);
            }
        }
        //get whole object from list
        public Login GetSelectedLoginObjectFromList()
        {
            Login loginToGet = new Login();
            IEnumerable<Login> loginList = loginApi.GetAllLogins();
            foreach (Login login in loginList)
            {
                if (login.UserName.Equals(listBoxLogins.SelectedItem))
                {
                    loginToGet = login;
                }
            }
            return loginToGet;
        }

        public void DeleteLogin()
        {
            if (GetSelectedLoginObjectFromList() != null)
            {
                loginApi.DeleteLogin(GetSelectedLoginObjectFromList());
            }
        }

        public void AddNewLogin()
        {
            if (textBoxCreateLoginUsername.Text.Length > 0 && textBoxCreateLoginUsername.Text.Length > 0)//we can add password creation rules here
            {
                Login login = new Login();
                login.UserName = textBoxCreateLoginUsername.Text;
                login.Password = textBoxCreateLoginUsername.Text;
                login.AdminRights = radioButtonAdminRightsYes.Checked;
                loginApi.AddLogin(login);
            }
            else
            {
                GetInputErrorMessageBox();
            }
        }

        public void ClearFieldsCreateLogin()
        {
            textBoxCreateLoginUsername.ResetText();
            textBoxCreateLoginPassword.ResetText();
            radioButtonAdminRightsNo.Checked = false;
            radioButtonAdminRightsYes.Checked = false;
        }
        // refresh login page
        public void Refresh()
        {
            listBoxLogins.Items.Clear();
            GetAllLogins();
        }
        #endregion
        #region Login Page Wiring
        private void buttonRefreshLogins_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void buttonCreateLoginFinish_Click(object sender, EventArgs e)
        {
            AddNewLogin();
            ClearFieldsCreateLogin();
            Refresh();
        }

        private void buttonDeleteLogin_Click(object sender, EventArgs e)
        {
            if (GetDeleteMessageBoxResult() == DialogResult.Yes)
            {
                DeleteLogin();
                Refresh();
            }
        }

        private void buttonCancelCreateLogin_Click(object sender, EventArgs e)
        {
            ClearFieldsCreateLogin();
        }
        #endregion

        #region Wiring
        private void tabControl1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void saleBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            
        }

        private void buttonRefreshSale_Click(object sender, EventArgs e)
        {
            
        }
        #endregion
    }
}
