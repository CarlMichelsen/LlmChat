import { navigateToLoginPage } from "../../util/client/loginClient";

const LogedOut: React.FC = () => {
    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
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

                    <button onClick={() => navigateToLoginPage("Development")}>development</button>
                </div>
            </div>
        </div>
    );
}

export default LogedOut;