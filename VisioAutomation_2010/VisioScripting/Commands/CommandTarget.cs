using VisioAutomation.Exceptions;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioScripting.Commands
{
    public class CommandTarget
    {
        public readonly Client Client;
        public Microsoft.Office.Interop.Visio.Application Application;
        public Microsoft.Office.Interop.Visio.Document ActiveDocument;
        public Microsoft.Office.Interop.Visio.Page ActivePage;

        public bool HasActiveApplication => this.Application != null;
        public bool HasActiveDocument => this.ActiveDocument != null;
        public bool HasActivePage => this.ActivePage != null;


        public CommandTarget(Client client)
        {
            this.Client = client;
        }

        public CommandTarget(Client client, CommandTargetFlags flags )
        {
            this.Client = client;

            check(flags);
        }

        public void Assert(CommandTargetFlags flags)
        {
            check(flags);
        }

        private void check(CommandTargetFlags flags)
        {
            bool require_app = (flags & CommandTargetFlags.Application) != 0;
            bool require_document = (flags & CommandTargetFlags.ActiveDocument) != 0;
            bool require_page = (flags & CommandTargetFlags.ActivePage) != 0;

            require_app = require_app || require_document || require_page;
            require_document = require_document || require_page;

            this.Application = this.Client.Application.VisioApplication;

            if (this.Application == null && require_app )
            {
                var has_app = this.Client.Application.VisioApplication != null;
                if (!has_app)
                {
                    throw new System.ArgumentException("CommandTarget: No Visio Application available");
                }
            }

            if (require_app && this.Application == null)
            {
                throw new VisioOperationException("CommandTarget: No Application");
            }

            if ((this.ActiveDocument == null) && require_document)
            {
                var doc = this.Application.ActiveDocument;

                if (doc == null)
                {
                    throw new VisioOperationException("CommandTarget: No Document");
                }

                this.ActiveDocument = doc;
                bool is_drawing = this.ActiveDocumentIsDrawing;
                if (is_drawing)
                {
                    this.Client.Output.WriteVerbose("CommandTarget: Verified a drawing is available for use");
                }
                else
                {
                    throw new VisioOperationException("CommandTarget: The Document is not a drawing document");
                }
            }

            if (this.ActiveDocument == null && require_document)
            {
                throw new VisioOperationException("CommandTarget: No Document");
            }

            if ((this.ActivePage == null) && ((flags & CommandTargetFlags.ActivePage) != 0))
            {
                if (this.Application == null)
                {
                    throw new VisioOperationException("CommandTarget: Internal error application should never be null in this case");
                }
                this.ActivePage = this.Application.ActivePage;
            }

            if (this.ActivePage == null && require_page)
            {
                throw new VisioOperationException("CommandTarget: No Page");
            }

        }

        public bool ActiveDocumentIsDrawing
        {
            get
            {
                // if there's no active document, then there can't be an active document
                if (this.ActiveDocument == null)
                {
                    this.Client.Output.WriteVerbose("ActiveDocumentIsDrawing: No Active Document");
                    return false;
                }

                var active_window = this.Application.ActiveWindow;

                // If there's no active window there can't be an active document
                if (active_window == null)
                {
                    this.Client.Output.WriteVerbose("ActiveDocumentIsDrawing: No Active Window");
                    return false;
                }

                // Check if the window type matches that of a document
                short active_window_type = active_window.Type;
                var vis_drawing = (int)IVisio.VisWinTypes.visDrawing;
                var vis_master = (int)IVisio.VisWinTypes.visMasterWin;
                // var vis_sheet = (short)IVisio.VisWinTypes.visSheet;

                this.Client.Output.WriteVerbose("ActiveDocumentIsDrawing: The Active Window: Type={0} & SybType={1}", active_window_type, active_window.SubType);
                if (!(active_window_type == vis_drawing || active_window_type == vis_master))
                {
                    this.Client.Output.WriteVerbose("ActiveDocumentIsDrawing: The Active Window Type must be one of {0} or {1}", IVisio.VisWinTypes.visDrawing, IVisio.VisWinTypes.visMasterWin);
                    return false;
                }

                var ap = this.Application.ActivePage;
                //  verify there is an active page
                if (ap == null)
                {
                    this.Client.Output.WriteVerbose("ActiveDocumentIsDrawing: No Active Page");

                    // 64 means master is being edited
                    if (active_window.SubType != 64)
                    {
                        this.Client.Output.WriteVerbose("ActiveDocumentIsDrawing: Window is not editing a master");
                        return false;
                    }
                }

                this.Client.Output.WriteVerbose("ActiveDocumentIsDrawing: Success. Verified a drawing is available for use");

                return true;
            }
        }
    }
}