import { useEffect } from 'react';
import { QueryClient, QueryClientProvider, useQuery } from 'react-query';
import { Provider, useSelector } from 'react-redux';
import store, { RootApplicationState } from './store';
import { getUser } from './util/client/userClient';
import { handleUserDataState } from './util/handler/userDataHandler';
import Pending from './components/LoginStates/Pending';
import LoggedIn from './components/LoginStates/LoggedIn';
import LoggedOut from './components/LoginStates/LoggedOut';
import Error from './components/LoginStates/Error';

const RootComponent: React.FC = () => {
	// Queries
	const { isLoading, isError, data } = useQuery('user', getUser, {
		staleTime: 1000 * 60 * 2,
		cacheTime: 60,
	});

	useEffect(() => {
		handleUserDataState(isLoading, isError, data);
	}, [isLoading, isError, data])

	const userState = useSelector((state: RootApplicationState) => state.user);

	switch (userState.authorized) {
		case "pending":
			return (<Pending />)
		case "logged-in":
			return (<LoggedIn />)
		case "logged-out":
			return (<LoggedOut />)
		default:
			return (<Error />)
	}
}

const queryClient = new QueryClient();
const App: React.FC = () => {
	return (
		<QueryClientProvider client={queryClient}>
			<Provider store={store}>
		  		<RootComponent />
			</Provider>
		</QueryClientProvider>
	)
}

export default App;
