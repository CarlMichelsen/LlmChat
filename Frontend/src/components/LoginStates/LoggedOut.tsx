import { navigateToLoginPage } from "../../util/client/loginClient";

const LoggedOut: React.FC = () => {
    return (
        <div className="min-h-screen flex items-center justify-center">
            <div className="bg-white dark:bg-zinc-700 p-8 rounded-lg shadow-md w-full max-w-md">
                <h1 className="text-3xl font-bold mb-6 text-center">Welcome to LLM Chat</h1>
                <div className="space-y-4">
                    <button
                        className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline flex items-center justify-center"
                        onClick={() => navigateToLoginPage("Discord")}
                    >
                        Log in with Discord
                    </button>

                    <button
                        className="w-full bg-gray-800 hover:bg-gray-900 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline flex items-center justify-center"
                        onClick={() => navigateToLoginPage("Github")}
                    >
                        Log in with GitHub
                    </button>

                    {import.meta.env.MODE == "development" && <button onClick={() => navigateToLoginPage("Development")} className="hover:underline hover:bg-zinc-900 text-center p-2 w-full rounded-md">Log in for Development</button>}
                </div>
            </div>
        </div>
    );
}

export default LoggedOut;