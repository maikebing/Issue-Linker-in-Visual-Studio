﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Issue_Linker
{
  /// <summary>
  /// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
  /// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
  /// </summary>
  [Export(typeof(IWpfTextViewCreationListener))]
  [ContentType("text")]
  [TextViewRole(PredefinedTextViewRoles.Document)]
  internal sealed class CreateVisualsTextViewCreationListener : IWpfTextViewCreationListener
  {
    // Disable "Field is never assigned to..." and "Field is never used" compiler's warnings. Justification: the field is used by MEF.
#pragma warning disable 649, 169

    /// <summary>
    /// Defines the adornment layer for the scarlet adornment. This layer is ordered
    /// after the selection layer in the Z-order
    /// </summary>
    [Export(typeof(AdornmentLayerDefinition))]
    [Name("CreateVisuals")]
    [Order(After = PredefinedAdornmentLayers.Caret)]
    private AdornmentLayerDefinition editorAdornmentLayer;

    public void TextViewCreated(IWpfTextView textView)
    {
    }

#pragma warning restore 649, 169

  }
}