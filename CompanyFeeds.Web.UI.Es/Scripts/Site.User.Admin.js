function deleteEntriesByUser(id, message)
{
	if (confirm(message))
	{
		$.post(deleteEntriesUrl, {id:id}, function(deleted){
			alert("Deleted " + deleted + " entries.");
		});
	}
	return false;
}

function deleteUser(id, message)
{
	if (confirm(message))
	{
		$.post(deleteUserUrl, {id:id}, function(){
			alert("User deleted");
		});
	}
	return false;
}