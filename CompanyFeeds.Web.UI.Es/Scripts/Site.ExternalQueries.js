var externalQueriesSources  = 
{
	crunchbase : 1
}
function QueryResult(){}QueryResult.prototype.tag = "";QueryResult.prototype.name = "";QueryResult.prototype.source = 0;QueryResult.prototype.description = "";QueryResult.prototype.url = "";

var crunchbase = 
{
	callback : function(){}
	,
	callbackNotResults : function(){}
	,
	getCallback : function(){}
	,
	query : function(value)
	{
		$.getJSON("http://api.crunchbase.com/v/1/search.js?query=" + value + ".com&callback=?", crunchbase.queryCallback);	
	}
	,
	queryCallback : function(query){
		var found = false;
		if (!query)
		{
			return;
		}
		if (!query.results)
		{
			return;
		}
		if (query.total > 0)
		{
			for (var i=0;i<query.results.length;i++)
			{
				var result = query.results[i];
				if (result.namespace == "company")
				{
					var queryResult = new QueryResult();
					queryResult.name = result.name;
					queryResult.tag = result.permalink;
					queryResult.source = externalQueriesSources.crunchbase;
					
					crunchbase.callback(queryResult);
					found = true;
					break;
				}
			}
		}
		if (!found)
		{
			crunchbase.callbackNotResults(externalQueriesSources.crunchbase);
		}
	}
	,
	getCallbackInternal : function (result)
	{
		//Adapt data
		var company = new QueryResult();
		company.tag = result.permalink;
		company.name = result.name;
		company.url = result.homepage_url;
		company.description = result.overview;
		
		crunchbase.getCallback(company);
	}
	,
	get : function (tag)
	{
		$.getJSON("http://api.crunchbase.com/v/1/company/" + tag + ".js?callback=?", crunchbase.getCallbackInternal);	
	}
};
var externalQueries = 
{
	currentSearch : ""
	,
	lastResult : null
	,
	lastResultQuery : null
	,
	confirmedUser : false
	,
	searchRejected : new Array()
	,
	foundCallback : function(){}
	,
	getSuccededCallback : function(){}
	,
	query : function (value, callBack)
	{
		if (value == null || value == "")
		{
			return;
		}
		
		if (externalQueries.currentSearch != "" || externalQueries.confirmedUser)
		{
			return;
		}
		if (externalQueries.lastResultQuery != value)
		{
			externalQueries.foundCallback = callBack;
			externalQueries.currentSearch = value;
			crunchbase.callback = externalQueries.queryCallback;
			crunchbase.callbackNotResults = externalQueries.queryNoResultsCallback;
			crunchbase.query(value);
		}
	}
	,
	queryCallback : function (result)
	{
		externalQueries.lastResult = result;
		externalQueries.lastResultQuery = externalQueries.currentSearch;
		externalQueries.currentSearch = "";
		
		externalQueries.foundCallback(result);
	}
	,
	queryNoResultsCallback : function (source)
	{
		if (source == externalQueriesSources.crunchbase)
		{
			externalQueries.lastResult = null; 
			externalQueries.currentSearch = "";
		}
	}
	,
	getCallback : function (company)
	{
		externalQueries.getSuccededCallback(company);
	}
	,
	get : function(callback)
	{
		externalQueries.getSuccededCallback = callback;
		crunchbase.getCallback = externalQueries.getCallback;
		crunchbase.get(externalQueries.lastResult.tag);
	}
}