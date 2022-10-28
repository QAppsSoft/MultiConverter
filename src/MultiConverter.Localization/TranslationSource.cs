using System.ComponentModel;
using System.Globalization;
using System.Resources;
using MultiConverter.Languages;

namespace MultiConverter.Localization;

public class TranslationSource : INotifyPropertyChanged
{
    private const string IndexerName = "Item";
    private const string IndexerArrayName = "Item[]";

    private readonly ResourceManager _resManager = Resources.ResourceManager;
    private CultureInfo _currentCulture;

    public static TranslationSource Instance { get; } = new();

    public string this[string key] => _resManager.GetString(key, _currentCulture);

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (Equals(_currentCulture, value))
            {
                return;
            }

            _currentCulture = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
