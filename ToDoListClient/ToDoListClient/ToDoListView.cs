using System;
using System.Windows.Forms;

namespace ToDoListClient
{
    /// <summary>
    /// View for ToDoListClient
    /// </summary>
    public partial class ToDoListView : Form
    {
        /// <summary>
        /// Creates the view
        /// </summary>
        public ToDoListView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// If state == true, enables all controls that are normally enabled; disables Cancel.
        /// If state == false, disables all controls; enables Cancel.
        /// </summary>
        public void EnableControls(bool state)
        {
            registerButton.Enabled = state;
            taskButton.Enabled = state && UserRegistered && taskBox.Text.Length > 0;
            allTaskButton.Enabled = state && UserRegistered;
            showCompletedTasksButton.Enabled = state && UserRegistered;

            foreach (Control control in taskPanel.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = state && UserRegistered;
                }
            }
            cancelButton.Enabled = !state;
        }

        /// <summary>
        /// Removes all displayed tasks.
        /// </summary>
        public void Clear()
        {
            taskPanel.Controls.Clear();
        }

        /// <summary>
        /// Adds a row to the task display.
        /// </summary>
        public void AddItem(string description, bool isCompleted, bool belongsToUser)
        {
            int row = taskPanel.Controls.Count / 3;
            taskPanel.Controls.Add(new Label() { Text = description });

            if (belongsToUser)
            {
                Button delete = new Button() { Text = "Delete" };
                taskPanel.Controls.Add(delete);
                delete.Click += (s, e) => FireDeleteEvent(row);
            }
            else
            {
                taskPanel.Controls.Add(new Label());
            }

            if (isCompleted)
            {
                taskPanel.Controls.Add(new Label() { Text = "Completed" });
            }
            else if (belongsToUser)
            {
                Button finished = new Button() { Text = "Complete" };
                taskPanel.Controls.Add(finished);
                finished.Click += (s, e) => FireDoneEvent(row);
            }
            else
            {
                taskPanel.Controls.Add(new Label());
            }
        }

        /// <summary>
        /// Backing variable for UserRegistered property
        /// </summary>
        private bool _userRegistered = false;

        /// <summary>
        /// Is the user currently registered?
        /// </summary>
        public bool UserRegistered
        {
            get { return _userRegistered; }
            set
            {
                _userRegistered = value;
                taskBox_TextChanged(null, null);
                showCompletedTasksButton.Enabled = true;
                allTaskButton.Enabled = true;
            }
        }

        /// <summary>
        /// Fired when user must be registered.
        /// Parameters are name and email.
        /// </summary>
        public event Action<string, string> RegisterPressed;

        /// <summary>
        /// Fired when new task must be submitted.
        /// Parameter is the task description.
        /// </summary>
        public event Action<string> SubmitPressed;

        /// <summary>
        /// Fired when one of the filter checkboxes is changed.
        /// Parameters are the states of the checkboxes.
        /// </summary>
        public event Action<bool, bool> FilterChanged;

        /// <summary>
        /// Fires when a task is to be deleted.
        /// Parameter is the line on which the task appears.
        /// </summary>
        public event Action<int> DeletePressed;

        /// <summary>
        /// Fires when a task is to marked as completed.
        /// Parameter is the line on which the task appears.
        /// </summary>
        public event Action<int> DonePressed;

        /// <summary>
        /// Fires when an ongoing action must be canceled.
        /// </summary>
        public event Action CancelPressed;

        private void registration_TextChanged(object sender, EventArgs e)
        {
            registerButton.Enabled = nameBox.Text.Trim().Length > 0 && emailBox.Text.Trim().Length > 0;
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (RegisterPressed != null)
            {
                RegisterPressed(nameBox.Text.Trim(), emailBox.Text.Trim());
            }
        }

        private void taskBox_TextChanged(object sender, EventArgs e)
        {
            taskButton.Enabled = UserRegistered && taskBox.Text.Trim().Length > 0;
        }

        private void taskButton_Click(object sender, EventArgs e)
        {
            if (SubmitPressed != null)
            {
                SubmitPressed(taskBox.Text.Trim());
            }
        }

        private void allTaskButton_CheckedChanged(object sender, EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(allTaskButton.Checked, showCompletedTasksButton.Checked);
            }
        }

        private void showCompletedTasksButton_CheckedChanged(object sender, EventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(allTaskButton.Checked, showCompletedTasksButton.Checked);
            }
        }

        private void FireDeleteEvent(int row)
        {
            if (DeletePressed != null)
            {
                DeletePressed(row);
            }
        }

        private void FireDoneEvent(int row)
        {
            if (DonePressed != null)
            {
                DonePressed(row);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (CancelPressed != null)
            {
                CancelPressed();
            }
        }
    }
}
