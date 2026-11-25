declare var bootstrap: {
    Modal: {
        new(element: Element): {
            show(): void;
            hide(): void;
        };
        getOrCreateInstance(element: Element): {
            show(): void;
            hide(): void;
        };
    };
};