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
                className="bg-white p-4 rounded-md w-full h-[750px] md:w-[750px]"
                open
            >
                {children}
            </dialog>
        </div>,
        document.body
    );
}

export default Dialog;