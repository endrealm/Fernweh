using Microsoft.Xna.Framework.Content;
using PipelineExtensionLibrary;

namespace Core.Content;

public class DialogLoader : ILoader<DialogTranslationData>
{
    private readonly ContentManager _contentManager;


    public DialogLoader(ContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    public DialogTranslationData Load(string file, IArchiveLoader archiveLoader)
    {
        return _contentManager.Load<DialogTranslationData>(file);
    }
}