/*
** Manipulate Dynamic Forms without Ajax
*/

function appendFormEntry(listId, entryClass){
  var newEntry = $('.' + entryClass + ':first').clone();
  // clear the inputs
  $('#' + listId).append(newEntry);
}

function removeFormEntry(input, entryClass){
  if ($('.' + entryClass).length > 1){
    $(input).closest('.' + entryClass).remove();
  }
}

function updateEntryIndex(listId, entryClass){
  // for each entry
  $('#' + listId + ' .' + entryClass).each(function(i){
    // for each input
    $(this).find(':input').each(function(){
      // fix the indexes
      updateInputId(this, i);
      updateInputName(this, i);
    });
  });
}

function updateInputId(input, newIndex){
  var matches = input.id.match(/(.+_)(\d+)(__.+)$/);
  if (matches && 4 == matches.length){
    input.id = matches[1] + newIndex + matches[3];
  }
}

function updateInputName(input, newIndex){
  var matches = input.name.match(/(.+\[)(\d+)(\].+)$/);
  if (matches && 4 == matches.length){
    input.name = matches[1] + newIndex + matches[3];
  }
}

