import { createPortal } from "react-dom";

type DialogProps = {
    isOpen: boolean;
    onClose: () => void;
    children: React.ReactNode;
}

const Dialog: React.FC<DialogProps> = ({ isOpen, onClose, children }) => {
    if (!isOpen) return null;

    const handleDialogClick = (event: React.MouseEvent<HTMLDialogElement>) => {
        event.stopPropagation();
    };

    return createPortal(
        <div
            className="fixed top-0 left-0 w-full h-full bg-black bg-opacity-70 flex items-center justify-center z-50 backdrop-blur-sm"
            onClick={onClose}
        >
            <dialog
                onClick={handleDialogClick}
                className="grid grid-rows-[1.5rem_1fr] text-black bg-white dark:text-white dark:bg-zinc-600 rounded-md w-full h-[750px] md:w-[750px]"
                open
            >
                <div className="grid grid-cols-[1fr_1.5rem]">
                    <div></div>

                    <div>
                        <button className="bg-red-600 hover:bg-red-800 hover:underline text-white w-6 h-6 rounded-tr-md rounded-bl-md" onMouseDown={onClose}>X</button>
                    </div>
                </div>
                <div className="mx-4 mb-4">
                    {children}
                </div>
            </dialog>
        </div>,
        document.body
    );
}

export default Dialog;