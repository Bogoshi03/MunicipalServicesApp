using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class EventsForm : Form
    {
        private class MunicipalEvent
        {
            public string Name;
            public string Category;
            public DateTime Date;
            public string Location;
            public string Description;
            public int Priority;

            public override string ToString()
            {
                return Date.ToString("dd MMM yyyy") + " | " +
                       Name + " | " + Category + " | " + Location;
            }
        }

        private List<MunicipalEvent> eventsList =
            new List<MunicipalEvent>();

        private SortedDictionary<DateTime, List<MunicipalEvent>>
            eventsByDate =
            new SortedDictionary<DateTime, List<MunicipalEvent>>();

        private Dictionary<string, List<MunicipalEvent>>
            eventsByCategory =
            new Dictionary<string, List<MunicipalEvent>>(
                StringComparer.OrdinalIgnoreCase);

        private HashSet<string> categories =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private Stack<string> recentSearches =
            new Stack<string>();

        private Queue<string> searchQueue =
            new Queue<string>();

        private SortedDictionary<int, Queue<MunicipalEvent>>
            priorityQueue =
            new SortedDictionary<int, Queue<MunicipalEvent>>();

        private TextBox txtSearch = new TextBox();
        private ComboBox cmbCategory = new ComboBox();
        private DateTimePicker dtpDate = new DateTimePicker();
        private CheckBox chkDate = new CheckBox();
        private ListBox lstEvents = new ListBox();
        private ListBox lstRecommendations = new ListBox();
        private Label lblStatus = new Label();

        public EventsForm()
        {
            InitializeComponent();

            Text = "Local Events and Announcements";
            Size = new Size(950, 650);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(750, 500);

            BuildInterface();
            LoadSampleEvents();
            DisplayEvents(eventsList);
        }

        private void BuildInterface()
        {
            Label heading = new Label();
            heading.Text = "Local Events and Announcements";
            heading.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            heading.Location = new Point(20, 15);
            heading.Size = new Size(650, 40);
            Controls.Add(heading);

            Label searchLabel = new Label();
            searchLabel.Text = "Search:";
            searchLabel.Location = new Point(20, 75);
            searchLabel.AutoSize = true;
            Controls.Add(searchLabel);

            txtSearch.Location = new Point(100, 70);
            txtSearch.Size = new Size(220, 25);
            Controls.Add(txtSearch);

            Label categoryLabel = new Label();
            categoryLabel.Text = "Category:";
            categoryLabel.Location = new Point(340, 75);
            categoryLabel.AutoSize = true;
            Controls.Add(categoryLabel);

            cmbCategory.Location = new Point(410, 70);
            cmbCategory.Size = new Size(160, 25);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.Add("All categories");
            cmbCategory.SelectedIndex = 0;
            Controls.Add(cmbCategory);

            chkDate.Text = "Filter by date";
            chkDate.Location = new Point(590, 72);
            chkDate.AutoSize = true;
            Controls.Add(chkDate);

            dtpDate.Location = new Point(700, 70);
            dtpDate.Size = new Size(150, 25);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Enabled = false;
            Controls.Add(dtpDate);

            chkDate.CheckedChanged += delegate
            {
                dtpDate.Enabled = chkDate.Checked;
            };

            Button btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(100, 110);
            btnSearch.Size = new Size(100, 32);
            btnSearch.Click += SearchEvents;
            Controls.Add(btnSearch);

            Button btnClear = new Button();
            btnClear.Text = "Show All";
            btnClear.Location = new Point(210, 110);
            btnClear.Size = new Size(100, 32);
            btnClear.Click += delegate
            {
                txtSearch.Clear();
                cmbCategory.SelectedIndex = 0;
                chkDate.Checked = false;
                DisplayEvents(eventsList);
                ShowRecommendations("");
                lblStatus.Text = "Showing all events.";
            };
            Controls.Add(btnClear);

            Label eventsLabel = new Label();
            eventsLabel.Text = "Available Events";
            eventsLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            eventsLabel.Location = new Point(20, 160);
            eventsLabel.AutoSize = true;
            Controls.Add(eventsLabel);

            lstEvents.Location = new Point(20, 190);
            lstEvents.Size = new Size(570, 300);
            lstEvents.Anchor = AnchorStyles.Top |
                               AnchorStyles.Bottom |
                               AnchorStyles.Left |
                               AnchorStyles.Right;
            Controls.Add(lstEvents);

            Label recommendationsLabel = new Label();
            recommendationsLabel.Text = "Recommended Events";
            recommendationsLabel.Font =
                new Font("Segoe UI", 11, FontStyle.Bold);
            recommendationsLabel.Location = new Point(610, 160);
            recommendationsLabel.AutoSize = true;
            Controls.Add(recommendationsLabel);

            lstRecommendations.Location = new Point(610, 190);
            lstRecommendations.Size = new Size(300, 300);
            lstRecommendations.Anchor = AnchorStyles.Top |
                                        AnchorStyles.Bottom |
                                        AnchorStyles.Right;
            Controls.Add(lstRecommendations);

            lblStatus.Location = new Point(20, 510);
            lblStatus.Size = new Size(650, 30);
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Controls.Add(lblStatus);

            Button btnHistory = new Button();
            btnHistory.Text = "Show Search History";
            btnHistory.Location = new Point(20, 550);
            btnHistory.Size = new Size(160, 32);
            btnHistory.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnHistory.Click += delegate
            {
                lstRecommendations.Items.Clear();

                foreach (string search in recentSearches)
                    lstRecommendations.Items.Add(search);

                if (recentSearches.Count == 0)
                    lstRecommendations.Items.Add("No searches yet.");
            };
            Controls.Add(btnHistory);

            Button btnPriority = new Button();
            btnPriority.Text = "Show Priority Events";
            btnPriority.Location = new Point(190, 550);
            btnPriority.Size = new Size(160, 32);
            btnPriority.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPriority.Click += delegate
            {
                lstRecommendations.Items.Clear();

                foreach (var group in priorityQueue)
                {
                    foreach (MunicipalEvent ev in group.Value)
                        lstRecommendations.Items.Add(
                            "Priority " + group.Key + ": " + ev.Name);
                }

                if (lstRecommendations.Items.Count == 0)
                    lstRecommendations.Items.Add("No priority events.");
            };
            Controls.Add(btnPriority);

            Button btnBack = new Button();
            btnBack.Text = "Back to Main Menu";
            btnBack.Location = new Point(750, 550);
            btnBack.Size = new Size(160, 32);
            btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnBack.Click += delegate { Close(); };
            Controls.Add(btnBack);
        }

        private void LoadSampleEvents()
        {
            AddEvent("Community Clean-Up",
                "Community", new DateTime(2026, 11, 7),
                "Community Hall", "Help clean public spaces.", 2);

            AddEvent("Youth Skills Workshop",
                "Education", new DateTime(2026, 11, 12),
                "Municipal Library", "Skills and career guidance.", 3);

            AddEvent("Water Services Update",
                "Utilities", new DateTime(2026, 11, 15),
                "Civic Centre", "Municipal water services briefing.", 1);

            AddEvent("Local Arts Festival",
                "Culture", new DateTime(2026, 11, 21),
                "Town Square", "Celebrate local artists and culture.", 4);

            AddEvent("Road Safety Awareness",
                "Community", new DateTime(2026, 11, 28),
                "Community Hall", "Road safety information session.", 2);

            foreach (string category in categories.OrderBy(c => c))
                cmbCategory.Items.Add(category);

            cmbCategory.SelectedIndex = 0;
        }

        private void AddEvent(string name, string category,
            DateTime date, string location, string description,
            int priority)
        {
            MunicipalEvent ev = new MunicipalEvent
            {
                Name = name,
                Category = category,
                Date = date,
                Location = location,
                Description = description,
                Priority = priority
            };

            eventsList.Add(ev);
            categories.Add(category);

            if (!eventsByDate.ContainsKey(date))
                eventsByDate[date] = new List<MunicipalEvent>();

            eventsByDate[date].Add(ev);

            if (!eventsByCategory.ContainsKey(category))
                eventsByCategory[category] =
                    new List<MunicipalEvent>();

            eventsByCategory[category].Add(ev);

            if (!priorityQueue.ContainsKey(priority))
                priorityQueue[priority] =
                    new Queue<MunicipalEvent>();

            priorityQueue[priority].Enqueue(ev);
        }

        private void SearchEvents(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            string category = cmbCategory.SelectedItem == null
                ? "All categories"
                : cmbCategory.SelectedItem.ToString();

            if (!string.IsNullOrWhiteSpace(term))
            {
                recentSearches.Push(term);
                searchQueue.Enqueue(term);
            }

            IEnumerable<MunicipalEvent> results = eventsList;

            if (!string.IsNullOrWhiteSpace(term))
            {
                results = results.Where(ev =>
                    ev.Name.IndexOf(term,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    ev.Description.IndexOf(term,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    ev.Location.IndexOf(term,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    ev.Category.IndexOf(term,
                        StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (category != "All categories")
            {
                results = results.Where(ev =>
                    ev.Category.Equals(category,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (chkDate.Checked)
            {
                results = results.Where(ev =>
                    ev.Date.Date == dtpDate.Value.Date);
            }

            List<MunicipalEvent> found = results
                .OrderBy(ev => ev.Date).ToList();

            DisplayEvents(found);
            ShowRecommendations(category == "All categories"
                ? term : category);

            lblStatus.Text = found.Count +
                " event(s) found. Searches recorded: " +
                searchQueue.Count;
        }

        private void DisplayEvents(IEnumerable<MunicipalEvent> items)
        {
            lstEvents.Items.Clear();

            foreach (MunicipalEvent ev in items)
                lstEvents.Items.Add(ev.ToString());

            if (lstEvents.Items.Count == 0)
                lstEvents.Items.Add("No events match your search.");
        }

        private void ShowRecommendations(string term)
        {
            lstRecommendations.Items.Clear();

            // Collect recent searches, newest first.
            List<string> searches = recentSearches
                .Take(10)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            // Include the current search if it is not already first.
            if (!string.IsNullOrWhiteSpace(term) &&
                (searches.Count == 0 ||
                 !searches[0].Equals(term,
                     StringComparison.OrdinalIgnoreCase)))
            {
                searches.Insert(0, term);
            }

            // Score events according to the user's search interests.
            var rankedEvents = eventsList.Select(ev =>
            {
                int score = 0;

                foreach (string search in searches)
                {
                    if (ev.Name.IndexOf(search,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 3;

                    if (ev.Category.IndexOf(search,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 3;

                    if (ev.Description.IndexOf(search,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 2;

                    if (ev.Location.IndexOf(search,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 1;
                }

                return new { Event = ev, Score = score };
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Event.Date)
            .Take(5)
            .ToList();

            foreach (var item in rankedEvents)
            {
                lstRecommendations.Items.Add(
                    item.Event.Name + " (" +
                    item.Event.Category + ") - Match score: " +
                    item.Score);
            }

            if (lstRecommendations.Items.Count == 0)
            {
                lstRecommendations.Items.Add(
                    "No recommendations yet. Try searching for an event.");
            }
        }
    }
}
