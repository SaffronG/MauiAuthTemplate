using System.ComponentModel;

namespace Auth0LightningLab.Maui.ViewModels
{
    public class MainPageViewModel(IShellClient shellClient) : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly IShellClient _shellClient = shellClient;
        public bool IsLoginViewVisible
        {
            get;
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoginViewVisible)));
            }
        } = true;
        public bool IsHomeViewVisible
        {
            get;
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsHomeViewVisible)));
            }
        } = false;
        public string Username
        {
            get;
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        } = String.Empty;
        public string UserPicture
        {
            get;
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserPicture)));
            }
        } = String.Empty;

        public Command LoginCommand => field ??= new(async () =>
        {
            throw new NotImplementedException();
        });
        public Command LogoutCommand => field ??= new(async () =>
        {
            throw new NotImplementedException();
        });
    }
}
