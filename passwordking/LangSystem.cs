using System.Collections.Generic;

namespace passwordking
{
    public class LangSystem
    {
        public Dictionary<string, string> Sets;

        public LangSystem(Language lang)
        {
            Sets = new Dictionary<string, string>();
            if (lang == Language.EN)
            {
                Sets.Add("title", "passwordKing");
                Sets.Add("new", "New");
                Sets.Add("load", "Load");

                Sets.Add("ui_add", "Add");
                Sets.Add("ui_edit", "Edit");
                Sets.Add("ui_delete", "Delete");

                Sets.Add("ui_copy", "Copy Password");
                Sets.Add("ui_save", "Save");
                Sets.Add("ui_exit", "Exit");

                Sets.Add("ui_scrollup", "Scroll Up");
                Sets.Add("ui_scrolldown", "Scroll Down");
                Sets.Add("ui_reset", "Reset");

                Sets.Add("ui_help", "Help");

                Sets.Add("ask_sure", "Are you sure?");

                Sets.Add("yes", "Yes");
                Sets.Add("no", "No");

                Sets.Add("name", "Name");
                Sets.Add("password", "Password");

                Sets.Add("mainpassword", "Please enter the Main Password");

                Sets.Add("textoverwrite", "Do you want to overwrite your Existing Passwords?");
                Sets.Add("textdelete", "Do you really want to delete all stored Passwords?");

                Sets.Add("textedit1", "Editing");
                Sets.Add("textedit2", "(Leave Empty to Edit nothing)");

                Sets.Add("textadd", "Add a new Entry");

                Sets.Add("helpTitle", "Help");

                Sets.Add("back", "Back");

                Sets.Add("howtoUseTitle", "How To Use");
                Sets.Add("configTitle", "Config");
                Sets.Add("creditTitle", "Credit");

                Sets.Add("howtoUseText", "How To Use");
                Sets.Add("configText", "Config");
                Sets.Add("creditText", "Credit");

                Sets.Add("arrow", "->");
            }
            else if (lang == Language.DE)
            {
                Sets.Add("title", "KennwörterKönig");
                Sets.Add("new", "Neu");
                Sets.Add("load", "Laden");

                Sets.Add("ui_add", "Hinzufügen");
                Sets.Add("ui_edit", "Bearbeiten");
                Sets.Add("ui_delete", "Löschen");

                Sets.Add("ui_copy", "Kennwörter Kopieren");
                Sets.Add("ui_save", "Speichern");
                Sets.Add("ui_exit", "Verlassen");

                Sets.Add("ui_scrollup", "Nach oben scrollen");
                Sets.Add("ui_scrolldown", "Nach unten scrollen");
                Sets.Add("ui_reset", "Zurücksetzen");

                Sets.Add("ui_help", "Hilfe");

                Sets.Add("ask_sure", "Sind sie sich sicher?");

                Sets.Add("yes", "Ja");
                Sets.Add("no", "Nein");

                Sets.Add("name", "Name");
                Sets.Add("password", "Kennwörter");

                Sets.Add("mainpassword", "Bitte geben sie das Hauptkennwörter ein");

                Sets.Add("textoverwrite", "Möchten Sie Ihre bestehenden Kennwörter überschreiben?");
                Sets.Add("textdelete", "Möchten Sie wirklich alle gespeicherten Kennwörter löschen?");

                Sets.Add("textedit1", "Bearbeiten");
                Sets.Add("textedit2", "(Leer lassen, um nichts zu bearbeiten)");

                Sets.Add("textadd", "Einen neuen Eintrag hinzufügen");

                Sets.Add("helpTitle", "Hilfe");

                Sets.Add("back", "Zurück");

                Sets.Add("howtoUseText", "How To Use");
                Sets.Add("configText", "Config");
                Sets.Add("creditText", "Credit");

                Sets.Add("howtoUseTitle", "How To Use");
                Sets.Add("configTitle", "Config");
                Sets.Add("creditTitle", "Credit");

                Sets.Add("arrow", "->");
            }
        }
        public enum Language
        {
            EN, DE
        }
    }
}
